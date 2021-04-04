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
        private string store_path;

        public List<ServiceLoginPassword> Items = new List<ServiceLoginPassword>();
        public void FillList(string text)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using StreamWriter streamWriter = new StreamWriter(memoryStream);

                streamWriter.Write(text);
                streamWriter.Flush();
                memoryStream.Position = 0;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string[] arr = streamReader.ReadLine().Split(':');
                        Items.Add(new ServiceLoginPassword(arr[0].Trim(), arr[1].Trim(), arr[2].Trim()));
                    }
                }

            }
        }
        public PasswordEnterWindow(string store_path)
        {
            InitializeComponent();
            this.store_path = store_path;
        }

        private void Enter_Password(object sender, RoutedEventArgs e)
        {
            string password = Password1.Password;
            Password = new NetworkCredential("", Password1.Password);
            
            try
            {
                CryptoFile cryptoFile = CryptoProtocol.Read(store_path);
                Crypter crypter = new Crypter(password, cryptoFile.Salt, cryptoFile.IV);
                string decrypt_data = crypter.Decrypt(cryptoFile.Cipher_text);
                byte[] hash = UsefulTools.ComputeSaltySHA256(Encoding.UTF8.GetBytes(decrypt_data), crypter.Salt);
                if (!StructuralComparisons.StructuralEqualityComparer.Equals(hash, cryptoFile.Hash))
                    throw new CryptographicException();
                FillList(decrypt_data);
                this.DialogResult = true;
            }
            catch (CryptographicException exp)
            {
                string wront_password = (string)Application.Current.FindResource("wrong_password_string");
                MessageBox.Show(wront_password, wront_password, MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (FileFormatException exp)
            {
                string file_isnot_store_string = (string)Application.Current.FindResource("file_isnot_store_string");
                MessageBox.Show(file_isnot_store_string, file_isnot_store_string, MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = true;
            }
        }
    }
}
