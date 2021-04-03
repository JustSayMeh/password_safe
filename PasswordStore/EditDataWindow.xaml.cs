using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для EditDataWindow.xaml
    /// </summary>
    public partial class EditDataWindow : Window
    {
        public ServiceLoginPassword serviceLoginPassword;
        public EditDataWindow(ServiceLoginPassword serviceLoginPassword)
        {
            InitializeComponent();
            PasswordM1.Password = serviceLoginPassword.Password.Password;
            PasswordM2.Password = serviceLoginPassword.Password.Password;
            PasswordV1.Text = serviceLoginPassword.Password.Password;
            PasswordV2.Text = serviceLoginPassword.Password.Password;
            ServiceName.Text = serviceLoginPassword.ServiceName;
            Login.Text = serviceLoginPassword.Login;
        }

        public EditDataWindow()
        {
            InitializeComponent();
            
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

        private void enter_Changes(object sender, RoutedEventArgs e)
        {
            string password1 = (checkBox1.IsChecked == true) ? PasswordV1.Text : PasswordM1.Password;
            string password2 = (checkBox2.IsChecked == true) ? PasswordV2.Text : PasswordM2.Password;
            if (!password1.Equals(password2))
            {
                MessageBox.Show("Пароли не совпадают!", "Пароли не совпадают!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
                
            serviceLoginPassword = new ServiceLoginPassword(ServiceName.Text, Login.Text, password1);
            this.DialogResult = true;
        }
    }
}
