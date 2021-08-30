using PasswordStore.Verificators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
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
    public partial class PasswordEnterWindow : Window
    {
        public NetworkCredential Password { get; private set; }


        public List<ServiceLoginPassword> Items = new List<ServiceLoginPassword>();

        private CryptoFile cryptoFile;
        public PasswordEnterWindow(CryptoFile cryptoFile)
        {
            InitializeComponent();
            this.cryptoFile = cryptoFile;
            Password1.Focus();
        }

        private void Enter_Password(object sender, RoutedEventArgs e)
        {
            string password = Password1.Password;
            Password = new NetworkCredential("", Password1.Password);
            try
            {
                if (!PasswordVerificator.PasswordIsValid(Factories.ProtocolVersions.LASTVERSION, Password, cryptoFile))
                    throw new CryptographicException();
                Crypter crypter = new Crypter(password, cryptoFile.Salt, cryptoFile.IV);
                string decrypt_data = crypter.Decrypt(cryptoFile.Cipher_text);
                Items = CryptoProtocol.FillList(decrypt_data);
                this.DialogResult = true;
            }
            catch (CryptographicException exp)
            {
                string wront_password = (string)Application.Current.FindResource("wrong_password_string");
                MessageBox.Show(wront_password, wront_password, MessageBoxButton.OK, MessageBoxImage.Error);
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
