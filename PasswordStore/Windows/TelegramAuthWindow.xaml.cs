using PasswordStore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace PasswordStore.Windows
{
    /// <summary>
    /// Логика взаимодействия для TelegramAuthWindow.xaml
    /// </summary>
    public partial class TelegramAuthWindow : Window
    {
        private bool codeSended = false;
        private string hash = "";
        public TelegramAuthWindow()
        {
            InitializeComponent();
            TelegramApiManager manager = TelegramApiManager.getInstance();
            manager.AuthorizationStateWaitCode += () =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CodeEnter.Visibility = Visibility.Visible;
                    PhoneNumber.IsEnabled = false;
                    codeSended = true;
                });
            };
            //this.DialogResult = false;
        }

        private async void Enter_Phone_Click(object sender, RoutedEventArgs e)
        {
          //  if (await TelegramApiManager.getInstance().IsPhoneRegisteredAsync(PhoneNumber.Text))
           // {
                TelegramApiManager.getInstance().SendCodeRequestAsync(PhoneNumber.Text);
          //  }
           // else
          //  {
             //   MessageBox.Show("Ошибка проверки номера", "Номер не заргистрирован в Telegram!\nПовторите ввод!", MessageBoxButton.OK, MessageBoxImage.Error);
          //  }
        }

        private async void Enter_Code_Click(object sender, RoutedEventArgs e)
        {
            try { 
                TelegramApiManager.getInstance().TryAuthAsync(Code.Text);
                this.DialogResult = true;
                this.Close();
            }catch (Exception exc)
            {
                MessageBox.Show("Неверный код", "Неверный код", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
            }
        }

        private void Change_Number_Click(object sender, RoutedEventArgs e)
        {
            CodeEnter.Visibility = Visibility.Collapsed;
            PhoneNumber.IsEnabled = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
