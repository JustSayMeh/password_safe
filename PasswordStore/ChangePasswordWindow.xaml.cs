using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordStore
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        string different_passwords = (string)Application.Current.FindResource("different_passwords_string");
        string wrong_password_string = (string)Application.Current.FindResource("wrong_password_string");
        
        private NetworkCredential Password;
        public NetworkCredential NewPassword;
        public ChangePasswordWindow(NetworkCredential password)
        {
            InitializeComponent();
            Password = password;
        }

        private void checkBox_Checked1(object sender, RoutedEventArgs e)
        {
            PasswordM1.ShowPassword();
            PasswordM2.ShowPassword();

        }

        private void checkBox_Unchecked1(object sender, RoutedEventArgs e)
        {
            PasswordM1.HiddenPassword();
            PasswordM2.HiddenPassword();
        }

        private void checkBox_Checked2(object sender, RoutedEventArgs e)
        {
            PasswordM3.ShowPassword();
            PasswordM4.ShowPassword();
        }

        private void checkBox_Unchecked2(object sender, RoutedEventArgs e)
        {
            PasswordM3.HiddenPassword();
            PasswordM4.HiddenPassword();
        }

        private void Enter_Changes(object sender, RoutedEventArgs e)
        {
            string password1 = PasswordM1.Password.Password;
            string password2 = PasswordM2.Password.Password;
            string password3 = PasswordM3.Password.Password;
            string password4 = PasswordM4.Password.Password;
            if (!password1.Equals(password2))
            {
                MessageBox.Show(different_passwords, different_passwords, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!password1.Equals(Password.Password))
            {
                MessageBox.Show(wrong_password_string, wrong_password_string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!password3.Equals(password4))
            {
                MessageBox.Show(different_passwords, different_passwords, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewPassword = new NetworkCredential("", password3);
            this.DialogResult = true;
        }

        private void Generate_New_Password(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorWindow window = new PasswordGeneratorWindow();
            window.ShowDialog();
        }
    }
}
