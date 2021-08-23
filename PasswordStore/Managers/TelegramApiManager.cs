using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TLSharp.Core;

namespace PasswordStore.Managers
{
    class TelegramApiManager
    {
        private string number;
        private TelegramClient client;
        private static Lazy<TelegramApiManager> instance = new Lazy<TelegramApiManager>(() => new TelegramApiManager());
        private TelegramApiManager(){}
        public static TelegramApiManager getInstance()
        {
            return instance.Value;
        }
        public async Task<TelegramApiManager> InitAsync()
        {
            client = new TelegramClient(7088232, "a75b2e5ef8a9005c06bd9d042046551d");
            await client.ConnectAsync();
            return this;
        }
        public async Task<string> SendCodeRequestAsync(string number)
        {
            this.number = number;
            return await client.SendCodeRequestAsync(number);
        }
        public async Task<TLUser> TryAuthAsync(string hash, string code)
        {
            return await client.MakeAuthAsync(number, hash, code);
        }

        public async Task<bool> IsPhoneRegisteredAsync(string phone_number)
        {
            return await client.IsPhoneRegisteredAsync(phone_number);
        }
    }
}
