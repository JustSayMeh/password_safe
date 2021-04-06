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
        private string different_passwords = (string) Application.Current.FindResource("different_passwords_string");
        private string empty_password_string = (string)Application.Current.FindResource("empty_password_string");
        private string space_password_string = (string)Application.Current.FindResource("space_password_string");
        public EditDataWindow(ServiceLoginPassword serviceLoginPassword)
        {
            InitializeComponent();
            PasswordM1.Password = serviceLoginPassword.Password;
            PasswordM2.Password = serviceLoginPassword.Password;
            ServiceName.Text = serviceLoginPassword.ServiceName;
            Login.Text = serviceLoginPassword.Login;
        }

        public EditDataWindow()
        {
            InitializeComponent();
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

        private void enter_Changes(object sender, RoutedEventArgs e)
        {
            string password1 = PasswordM1.Password.Password;
            string password2 = PasswordM2.Password.Password;
            if (!password1.Equals(password2))
            {
                MessageBox.Show(different_passwords, different_passwords, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (password1.Length == 0 || password2.Length == 0)
            {
                MessageBox.Show(empty_password_string, empty_password_string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (password1.Contains(" ") || password2.Contains(" ") )
            {
                MessageBox.Show(space_password_string, space_password_string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            serviceLoginPassword = new ServiceLoginPassword(ServiceName.Text, Login.Text, password1);
            this.DialogResult = true;
        }

        private void Generate_New_Password(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorWindow window = new PasswordGeneratorWindow();
            window.ShowDialog();
        }
    }
}
