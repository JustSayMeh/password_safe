using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для PasswordGeneratorWindow.xaml
    /// </summary>
    public partial class PasswordGeneratorWindow : Window
    {
        public PasswordGeneratorWindow()
        {
            InitializeComponent();
        }

        private void Generate_Password(object sender, RoutedEventArgs e)
        {
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
            return;
        }
    }
}
