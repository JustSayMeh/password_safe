using PasswordStore.Managers;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            TelegramApiManager manager = TelegramApiManager.GetInstance();
            manager.AuthorizationStateWaitCode += () =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CodeEnter.Visibility = Visibility.Visible;
                    PhoneNumber.IsEnabled = false;
                    codeSended = true;
                });
            };
        }

        private void Enter_Phone_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Password.Equals(password.Password))
            {
                TelegramApiManager.GetInstance().SendCodeRequestAsync(PhoneNumber.Text);
            } else
            {
                MessageBox.Show("Неверный пароль", "Неверный пароль", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private async void Enter_Code_Click(object sender, RoutedEventArgs e)
        {
                bool success = await TelegramApiManager.GetInstance().TryAuthAsync(Code.Text);
                string name = await TelegramApiManager.GetInstance().GetUserNameAsync();
                if (success)
                {
                    MessageBox.Show("Авторизация успешна", $"Вы авторизовались как {name} !", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void PhoneNumber_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            text.Text = formatNumber(text.Text);
        }

        private string formatNumber(string number)
        {
            StringBuilder bf = new StringBuilder();
            int stringLength = number.Length;
            if (stringLength > 0)
                bf.Append("+");
            bf.Append(number);
            return bf.ToString();
        }
    }
}
