using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordStore
{
    public class WindowCommands
    {
        static WindowCommands()
        {
            Ffa = new RoutedCommand("Ffa", typeof(TextBlock));
        }
        public static RoutedCommand Ffa { get; set; }
    }
}
