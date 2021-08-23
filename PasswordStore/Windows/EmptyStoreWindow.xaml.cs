using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
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
    /// Логика взаимодействия для PasswordEnterWindow.xaml
    /// </summary>
    public partial class EmptyStoreWindow : Window
    {
        private string different_passwords_string = (string)Application.Current.FindResource("different_passwords_string");
       

        public NetworkCredential Password { get; private set; }
        public EmptyStoreWindow()
        {
            InitializeComponent();
        }

        private void Enter_Password(object sender, RoutedEventArgs e)
        {
            string password1 = Password1.Password;
            string password2 = Password2.Password;
            if (password1.Equals(password2))
            {
                Password = new NetworkCredential("", Password1.SecurePassword);
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show(different_passwords_string);
            }
             
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Enter_Password(null, null);
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
