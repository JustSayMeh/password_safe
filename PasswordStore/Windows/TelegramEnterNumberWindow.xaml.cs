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
    /// Логика взаимодействия для TelegramAuthWindow.xaml
    /// </summary>
    public partial class TelegramAuthWindow : Window
    {
        public TelegramAuthWindow()
        {
            InitializeComponent();
        }

        private async void enter_button_Click(object sender, RoutedEventArgs e)
        {
            await TelegramApiManager.getInstance().InitAsync();
            if (await TelegramApiManager.getInstance().IsPhoneRegisteredAsync(PhoneNumber.Text))
            {
                string hash = await TelegramApiManager.getInstance().SendCodeRequestAsync(PhoneNumber.Text);
                TelegramCheckCodeWindow window = new TelegramCheckCodeWindow(hash);
                this.Close();
                window.Show(); 
            }
            else
            {

            }
        }
    }
}
