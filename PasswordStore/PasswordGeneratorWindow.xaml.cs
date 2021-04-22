using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Логика взаимодействия для PasswordGeneratorWindow.xaml
    /// </summary>
    public partial class PasswordGeneratorWindow : Window
    {
        private string generate_string = (string)Application.Current.FindResource("generate_string");
        private string copy_string = (string)Application.Current.FindResource("copy_string_2");
        private string copy_to_clipboard_string = (string)Application.Current.FindResource("copy_to_clipboard_string");
        private NetworkCredential password_ = null;
        public PasswordGeneratorWindow()
        {
            InitializeComponent();
        }

        private void Generate_Password(object sender, RoutedEventArgs e)
        {
            if (password_ != null)
            {
                Clipboard.SetText(password_.Password);
            }

            PasswordGenerator passwordGenerator = new PasswordGenerator();
            if (Uppers.IsChecked == true)
                passwordGenerator.Enable_latin_upper();
            if (Kyrilic.IsChecked == true)
            {
                if (Uppers.IsChecked == true)
                    passwordGenerator.Enable_kyrilic_upper();
                passwordGenerator.Enable_kyrilic();
            }
            if (Numeric.IsChecked == true)
            {
                passwordGenerator.Enable_numbers();
            }
            if (Specials.IsChecked == true)
            {
                passwordGenerator.Enable_special_symbols();
            }
       
            NetworkCredential password = passwordGenerator.Generate((int)PasswordLengthSlider.Value);
            Clipboard.SetText(password.Password);
            password_ = password;
            GButton.Content = copy_string;
            statusBar.Text = copy_to_clipboard_string;
            ClearTimer timer = new ClearTimer(3000, statusBar, copy_to_clipboard_string);
            return;
        }

        private void GButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (password_ != null)
            {
                password_ = null;
                GButton.Content = generate_string;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
