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
            PasswordM1.Visibility = System.Windows.Visibility.Collapsed;
            PasswordV1.Visibility = System.Windows.Visibility.Visible;
            PasswordV1.Text = PasswordM1.Password;
        }

        private void checkBox_Unchecked1(object sender, RoutedEventArgs e)
        {
            PasswordV1.Visibility = System.Windows.Visibility.Collapsed;
            PasswordM1.Visibility = System.Windows.Visibility.Visible;
            PasswordM1.Password = PasswordV1.Text;
        }

        private void checkBox_Checked2(object sender, RoutedEventArgs e)
        {
            PasswordM2.Visibility = System.Windows.Visibility.Collapsed;
            PasswordV2.Visibility = System.Windows.Visibility.Visible;
            PasswordV2.Text = PasswordM2.Password;
        }

        private void checkBox_Unchecked2(object sender, RoutedEventArgs e)
        {
            PasswordV2.Visibility = System.Windows.Visibility.Collapsed;
            PasswordM2.Visibility = System.Windows.Visibility.Visible;
            PasswordM2.Password = PasswordV2.Text;
        }
        private void checkBox_Checked3(object sender, RoutedEventArgs e)
        {
            PasswordM3.Visibility = System.Windows.Visibility.Collapsed;
            PasswordV3.Visibility = System.Windows.Visibility.Visible;
            PasswordV3.Text = PasswordM1.Password;
        }

        private void checkBox_Unchecked3(object sender, RoutedEventArgs e)
        {
            PasswordV3.Visibility = System.Windows.Visibility.Collapsed;
            PasswordM3.Visibility = System.Windows.Visibility.Visible;
            PasswordM3.Password = PasswordV1.Text;
        }

        private void checkBox_Checked4(object sender, RoutedEventArgs e)
        {
            PasswordM4.Visibility = System.Windows.Visibility.Collapsed;
            PasswordV4.Visibility = System.Windows.Visibility.Visible;
            PasswordV4.Text = PasswordM2.Password;
        }

        private void checkBox_Unchecked4(object sender, RoutedEventArgs e)
        {
            PasswordV4.Visibility = System.Windows.Visibility.Collapsed;
            PasswordM4.Visibility = System.Windows.Visibility.Visible;
            PasswordM4.Password = PasswordV2.Text;
        }

        private void Enter_Changes(object sender, RoutedEventArgs e)
        {
            string password1 = (checkBox1.IsChecked == true) ? PasswordV1.Text : PasswordM1.Password;
            string password2 = (checkBox2.IsChecked == true) ? PasswordV2.Text : PasswordM2.Password;
            string password3 = (checkBox3.IsChecked == true) ? PasswordV3.Text : PasswordM3.Password;
            string password4 = (checkBox4.IsChecked == true) ? PasswordV4.Text : PasswordM4.Password;
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
