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
using Microsoft.Win32;
using PasswordStore.Factories;

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
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Call_Password_Window(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = FileDialog.CreateDefaultDialog(false);
            if (openFileDialog.ShowDialog() == true)
            {
                store_path = openFileDialog.FileName;
                try
                {
                    CryptoFile cryptoFile = CryptoProtocol.Read(store_path);
                    PasswordEnterWindow passwordEnterWindow = new PasswordEnterWindow(cryptoFile);
                    if (passwordEnterWindow.ShowDialog() == true)
                    {
                        Window window = new MainWindow(passwordEnterWindow.Password, passwordEnterWindow.Items, store_path);
                        window.Show();
                        this.Close();
                        return;
                    }
                }
                catch (FileFormatException exp)
                {
                    string file_isnot_store_string = (string)Application.Current.FindResource("file_isnot_store_string");
                    MessageBox.Show(file_isnot_store_string, file_isnot_store_string, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private void Call_Empty_Store_Window(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = FileDialog.CreateDefaultDialog(true);
            if (openFileDialog.ShowDialog() == true)
            {
                store_path = openFileDialog.FileName;
                EmptyStoreWindow emptyStoreWindow = new EmptyStoreWindow();
                if (emptyStoreWindow.ShowDialog() == true)
                {
                    CryptoFile cryptoFile = CryptoFileFactory.CreateCryptoFile(ProtocolVersions.LASTVERSION, emptyStoreWindow.Password, "");
                    CryptoProtocol.Save(cryptoFile, store_path);
                    Window window = new MainWindow(emptyStoreWindow.Password, new List<ServiceLoginPassword>(), store_path);
                    window.Show();
                    this.Close();
                    return;
                }
            }
            
        }

        private void Call_Password_Generator_Window(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorWindow window = new PasswordGeneratorWindow();
            window.ShowDialog();
        }
    }
}
