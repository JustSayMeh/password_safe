using PasswordStore.Factories;
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
        private string search_string = (string)Application.Current.FindResource("search_string");
        private string copy_to_clipboard_string = (string)Application.Current.FindResource("copy_to_clipboard_string");
        private string clear_clipboard_string = (string)Application.Current.FindResource("clear_clipboard_string");
        private string change_string = (string)Application.Current.FindResource("change_string");
        private string remove_string = (string)Application.Current.FindResource("remove_string");
        private string added_string = (string)Application.Current.FindResource("added_string");

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
            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight - toolBar.Height - menuPanel.ActualHeight - statusBar.ActualHeight - 25 < 100)
                ItemList.Height = 100;
            else
                ItemList.Height = this.ActualHeight - toolBar.Height - menuPanel.ActualHeight - statusBar.ActualHeight - 50;
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
                ClearTimer timer = new ClearTimer(3000, statusBar, change_string);
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
                ClearTimer timer = new ClearTimer(3000, statusBar, remove_string);
            }

        }

        private void Button_Click_Copy(object sender, RoutedEventArgs e)
        {
            ServiceLoginPassword dataRowView = (ServiceLoginPassword)((Button)e.Source).DataContext;
            Clipboard.SetText(dataRowView.Password.Password);
            ClearTimer timer = new ClearTimer(3000, statusBar, copy_to_clipboard_string);
        }

        public void storeData()
        {
            Crypter crypter = new Crypter(MasterPassword.Password);
            StringBuilder str = new StringBuilder();
            foreach (ServiceLoginPassword slp in items)
            {
                str.Append($"{slp.ServiceName} : {slp.Login} : {slp.Password.Password}\n");
            }
            CryptoFile cryptoFile = CryptoFileFactory.CreateCryptoFile(ProtocolVersions.LASTVERSION, MasterPassword, str.ToString());
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
                ClearTimer timer = new ClearTimer(3000, statusBar, added_string);
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
            ClearTimer timer = new ClearTimer(3000, statusBar, clear_clipboard_string);
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

        private void Change_Password(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(MasterPassword);
            if(changePasswordWindow.ShowDialog() == true)
            {
                MasterPassword = changePasswordWindow.NewPassword;
                storeData();
                ClearTimer timer = new ClearTimer(3000, statusBar, change_string);
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string mask = SearchBox.Text;
            if (mask.Length == 0)
                ItemList.ItemsSource = items;
            ItemList.ItemsSource = items.Where(item => item.Login.StartsWith(mask) || item.ServiceName.StartsWith(mask));
        }



        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text.Equals(search_string))
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black;
            }
           
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text.Length == 0)
            {
                SearchBox.Foreground = Brushes.Gray;
                SearchBox.Text = search_string;
            }
        }

        private void About(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }
    }
}
