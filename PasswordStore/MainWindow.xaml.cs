using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordStore
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public class ServiceLoginPassword
    {
        public string ServiceName { get; set; }
        public string Login { get; set; }
        public NetworkCredential Password { get; set; }

  
        public ServiceLoginPassword(string ServiceName, string Login, string Password)
        {
            this.ServiceName = ServiceName;
            this.Login = Login;
            this.Password = new NetworkCredential("", Password);
        }
    }
    public partial class MainWindow : Window
    {
        private string delete_row_string = (string)Application.Current.FindResource("delete_row_string");
        private string delete_string = (string)Application.Current.FindResource("delete_string");
        

        private NetworkCredential MasterPassword;
        List<ServiceLoginPassword> items = new List<ServiceLoginPassword>();
        string store_path;
        public MainWindow(NetworkCredential password, List<ServiceLoginPassword> items, string store_path)
        {
            InitializeComponent();
            this.items = items;
            ItemList.ItemsSource = items;
            MasterPassword = password;
            this.store_path = store_path;
        }

        private void TextBox_CopyToClipboard(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(((TextBox)sender).Text);
            ((TextBox)sender).SelectAll();
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            ServiceLoginPassword dataRowView = (ServiceLoginPassword)((Button)e.Source).DataContext;
            EditDataWindow window = new EditDataWindow(dataRowView);
            if (window.ShowDialog() == true)
            {
                if (dataRowView.ServiceName.Equals(window.serviceLoginPassword.ServiceName) &&
                    dataRowView.Login.Equals(window.serviceLoginPassword.Login) &&
                    dataRowView.Password.Password.Equals(window.serviceLoginPassword.Password.Password))
                    return;
                dataRowView.ServiceName = window.serviceLoginPassword.ServiceName;
                dataRowView.Login = window.serviceLoginPassword.Login;
                dataRowView.Password = window.serviceLoginPassword.Password;
                ItemList.Items.Refresh();
                storeData();
            }
        }
        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            ServiceLoginPassword dataRowView = (ServiceLoginPassword)((Button)e.Source).DataContext;
            MessageBoxResult result = MessageBox.Show(delete_row_string, delete_string, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                items.Remove(dataRowView);
                ItemList.Items.Refresh();
                storeData();
            }

        }

        private void Button_Click_Copy(object sender, RoutedEventArgs e)
        {
            ServiceLoginPassword dataRowView = (ServiceLoginPassword)((Button)e.Source).DataContext;
            Clipboard.SetText(dataRowView.Password.Password);
        }

        public void storeData()
        {
            Crypter crypter = new Crypter(MasterPassword.Password);
            StringBuilder str = new StringBuilder();
            foreach (ServiceLoginPassword slp in items)
            {
                str.Append($"{slp.ServiceName} : {slp.Login} : {slp.Password.Password}\n");
            }
            byte[] encrypted = crypter.Encrypt(str.ToString());
            byte[] salty = UsefulTools.ComputeSaltySHA256(Encoding.UTF8.GetBytes(str.ToString()), crypter.Salt);
            CryptoFile cryptoFile = new CryptoFile(encrypted, salty, crypter.IV, crypter.Salt);
            CryptoProtocol.Save(cryptoFile, store_path);
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            EditDataWindow window = new EditDataWindow();
            if (window.ShowDialog() == true)
            {
                items.Add(window.serviceLoginPassword);
                ItemList.Items.Refresh();
                storeData();
            }
        }

        private void Button_Click_Exit_To_Start(object sender, RoutedEventArgs e)
        {
            StartWindow window = new StartWindow();
            Clipboard.SetText("");
            window.Show();
            this.Close();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        { 
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText("");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Clipboard.SetText("");
            base.OnClosing(e);
        }

        private void Generate_New_Password(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorWindow window = new PasswordGeneratorWindow();
            window.ShowDialog();
        }
    }
}
