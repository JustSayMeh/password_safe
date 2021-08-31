using PasswordStore.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordStore
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        async void App_Startup(object sender, StartupEventArgs e)
        {
            await TelegramApiManager.GetInstance().InitAsync();
            StartWindow window = new StartWindow();
            window.Show();
        }
    }
}
