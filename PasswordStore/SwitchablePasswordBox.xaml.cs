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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordStore
{
    /// <summary>
    /// Логика взаимодействия для SwitchablePasswordBox.xaml
    /// </summary>
    public partial class SwitchablePasswordBox : UserControl
    {
        private bool hidden = true;
        public SwitchablePasswordBox()
        {
            InitializeComponent();
        }
        public void ShowPassword()
        {
            if (!hidden)
                return;
            PasswordM1.Visibility = System.Windows.Visibility.Collapsed;
            PasswordV1.Visibility = System.Windows.Visibility.Visible;
            PasswordV1.Text = PasswordM1.Password;
            hidden = false;
        }

        public void HiddenPassword()
        {
            if (hidden)
                return;
            PasswordV1.Visibility = System.Windows.Visibility.Collapsed;
            PasswordM1.Visibility = System.Windows.Visibility.Visible;
            PasswordM1.Password = PasswordV1.Text;
            hidden = true;
        }

        public NetworkCredential Password
        {
            get
            {
                string pass = (hidden) ? PasswordM1.Password : PasswordV1.Text;
                return new NetworkCredential("", pass);
            }
            set
            {
                PasswordM1.Password = value.Password;
                if (!hidden)
                {
                    PasswordV1.Text = PasswordM1.Password;
                }
            }
        }
    }
}
