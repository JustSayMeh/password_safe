using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private string open_store_string = (string)Application.Current.FindResource("open_store_string");
        private string create_empty_store_string = (string)Application.Current.FindResource("create_empty_store_string");
        private string store_path = ".store";
        public StartWindow()
        {
            InitializeComponent();
            if (File.Exists(store_path))
            {
                ActionButton.Click += Call_Password_Window;
                ActionButton.Content = open_store_string;
            }
            else
            {
                ActionButton.Click += Call_Empty_Store_Window;
                ActionButton.Content = create_empty_store_string;
            }

        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Call_Password_Window(object sender, RoutedEventArgs e)
        {
            PasswordEnterWindow passwordEnterWindow = new PasswordEnterWindow(store_path);
            if (passwordEnterWindow.ShowDialog() == true)
            {

                Window window = new MainWindow(passwordEnterWindow.Password, passwordEnterWindow.Items, store_path);
                window.Show();
                this.Close();

                return;
            }
        }

        private void Call_Empty_Store_Window(object sender, RoutedEventArgs e)
        {
            EmptyStoreWindow emptyStoreWindow = new EmptyStoreWindow();
            if (emptyStoreWindow.ShowDialog() == true)
            {
                Crypter crypter = new Crypter(emptyStoreWindow.Password.Password);
                byte[] encrypted = crypter.Encrypt("");
                byte[] salty = UsefulTools.ComputeSaltySHA256(Encoding.UTF8.GetBytes(""), crypter.Salt);
                CryptoFile cryptoFile = new CryptoFile(encrypted, salty, crypter.IV, crypter.Salt);
                CryptoProtocol.Save(cryptoFile, store_path);
                Window window = new MainWindow(emptyStoreWindow.Password, new List<ServiceLoginPassword>(), store_path);
                window.Show();
                this.Close();
                return;
            }
            
        }

        private void Call_Password_Generator_Window(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorWindow window = new PasswordGeneratorWindow();
            window.ShowDialog();
        }
    }
}
