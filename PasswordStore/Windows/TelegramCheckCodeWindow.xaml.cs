using PasswordStore.Managers;
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

namespace PasswordStore.Windows
{
    /// <summary>
    /// Логика взаимодействия для TelegramCheckCodeWindow.xaml
    /// </summary>
    public partial class TelegramCheckCodeWindow : Window
    {
        string hash;
        public TelegramCheckCodeWindow(string hash)
        {
            InitializeComponent();
            this.hash = hash;
        }

        private async void enter_button_Click(object sender, RoutedEventArgs e)
        {
            await TelegramApiManager.getInstance().TryAuthAsync(hash, Code.Text);
            this.Close();
        }
    }
}
