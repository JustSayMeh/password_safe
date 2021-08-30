using PasswordStore.Managers;
using System.Net;
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
        private NetworkCredential password;
        public TelegramAuthWindow(NetworkCredential password)
        {
            InitializeComponent();
            this.password = password;
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
            if (Password.Password.Equals(password.Password))
            {
                TelegramApiManager.getInstance().SendCodeRequestAsync(PhoneNumber.Text);
            } else
            {
                MessageBox.Show("Неверный пароль", "Неверный пароль", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private async void Enter_Code_Click(object sender, RoutedEventArgs e)
        {
                bool success = await TelegramApiManager.getInstance().TryAuthAsync(Code.Text);
                if (success)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
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
