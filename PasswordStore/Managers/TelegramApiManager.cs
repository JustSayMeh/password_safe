using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TdLib;
using static TdLib.TdApi;
using static TdLib.TdApi.InputFile;
using static TdLib.TdApi.InputMessageContent;
using static TdLib.TdApi.MessageContent;

namespace PasswordStore.Managers
{

    class TelegramApiManager
    {
        private string number;
        private TdClient client;
        private static Lazy<TelegramApiManager> instance = new Lazy<TelegramApiManager>(() => new TelegramApiManager());
        private bool hasAuth = false;
        private TelegramApiManager(){}
        public event Action AuthorizationStateWaitPhoneNumber;
        public event Action AuthorizationStateWaitCode;
        public event Action AuthorizationStateReady;
        public event Action AuthorizationStateLoggingOut;
        public static TelegramApiManager getInstance()
        {
            return instance.Value;
        }
        public async Task<TelegramApiManager> InitAsync()
        {
            client = new TdClient();
            client.UpdateReceived += handler;
            await client.SetLogVerbosityLevelAsync(1);
            
            return this;
        }
        public async void SendCodeRequestAsync(string number)
        {
            this.number = number;
            await client.SetAuthenticationPhoneNumberAsync(number);
        }
        public async void TryAuthAsync(string code)
        {
            await client.CheckAuthenticationCodeAsync(code);
        }

        public async Task<bool> IsPhoneRegisteredAsync(string phone_number)
        {
            return false;
        }
        public bool IsUserAuthorized()
        {
            return hasAuth;
        }

        public async void SendFileToMyselfAsync(string path)
        {
            var user = await client.GetMeAsync();
            var chat = await client.CreatePrivateChatAsync(user.Id);
            InputMessageDocument content = new InputMessageDocument();
            InputFileLocal file = new InputFileLocal();
            file.Path = path;
            file.DataType = "inputFileLocal";
            content.DisableContentTypeDetection = false;
            content.Document = file;
            content.DataType = "inputMessageDocument";
            content.Extra = "1";
            await client.SendMessageAsync(chat.Id, 0, 0, null, null, content);
        }

        public async Task<byte[]> LoadFileAsync(string path)
        {
            var fileInfo = new FileInfo(path);
            string filename = fileInfo.Name;
            var response = await GetFileVaultFiles();
            var file = response.Select(e => e.Document).Where(e => e.FileName.Contains(filename)).Select(e => e.Document_).DefaultIfEmpty(new TdApi.File()).First();
            if (file.Size == 0)
                return new byte[0];
            var bytes = await client.ReadFilePartAsync(file.Id, 0, file.Size);
            return bytes.Data;

        }

        private TdlibParameters ConfigureParameters()
        {
            var parameters = new TdlibParameters();
            parameters.UseFileDatabase = false;
            parameters.UseChatInfoDatabase = false;
            parameters.UseMessageDatabase = false;
            parameters.DatabaseDirectory = @"C:\Users\Oleg\source\repos\TelegramApiTest\TelegramApiTest\storage\";
            parameters.ApiHash = "a75b2e5ef8a9005c06bd9d042046551d";
            parameters.ApiId = 7088232;
            parameters.SystemLanguageCode = "ru";
            parameters.ApplicationVersion = "1.0.0";
            parameters.DeviceModel = "Desktop";
            return parameters;
        }


        private async Task<IList<MessageDocument>> GetFileVaultFiles()
        {
            var user = await client.GetMeAsync();
            var chat = await client.CreatePrivateChatAsync(user.Id);
            List<MessageDocument> list = new List<MessageDocument>();
            var messages = await client.GetChatHistoryAsync(chatId: chat.Id, limit: 99, fromMessageId: 0);
            while (messages.TotalCount > 0)
            {
                foreach (var th in messages.Messages_)
                {
                    if (th.Content is MessageDocument)
                    {
                        var content = (MessageDocument)th.Content;
                        if (content.Document.MimeType.Equals("vault"))
                            list.Add(content);
                    }
                }
                var last = messages.Messages_[messages.Messages_.Length - 1];
                messages = await client.GetChatHistoryAsync(chatId: chat.Id, limit: 50, fromMessageId: last.Id);
            }
            return list;
        }

        private void handler(object sender, TdApi.Update e)
        {
            if (e is TdApi.Update.UpdateAuthorizationState)
            {
                UpdateAuthorizationState(((TdApi.Update.UpdateAuthorizationState)e).AuthorizationState);
            }
            return;
        }


        private void UpdateAuthorizationState(AuthorizationState state)
        {
            if (state is AuthorizationState.AuthorizationStateWaitTdlibParameters)
            {
                var parameters = ConfigureParameters();
                client.SetTdlibParametersAsync(parameters);
            }
            else if (state is AuthorizationState.AuthorizationStateWaitEncryptionKey)
            {
                client.CheckDatabaseEncryptionKeyAsync();
            }
            else if (state is AuthorizationState.AuthorizationStateWaitPhoneNumber)
            {
                AuthorizationStateWaitPhoneNumber?.Invoke();
            }
            else if (state is AuthorizationState.AuthorizationStateWaitCode)
            {
                AuthorizationStateWaitCode?.Invoke();
            }
            else if (state is AuthorizationState.AuthorizationStateReady)
            {
                hasAuth = true;
                AuthorizationStateReady?.Invoke();
            }
            else if (state is AuthorizationState.AuthorizationStateLoggingOut)
            {
                hasAuth = false;
                AuthorizationStateLoggingOut?.Invoke();
            }
        }
    }
}
