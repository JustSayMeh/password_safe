using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordStore
{
    class FileDialog
    {
        private static string default_path = $"C:\\Users\\{Environment.UserName}\\Documents\\My Vaults";
        private static string vault_string = (string)Application.Current.FindResource("vault_string");
        public static OpenFileDialog CreateDefaultDialog(bool create_mode)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt += "vault";
            if (!Directory.Exists(default_path))
            {
                Directory.CreateDirectory(default_path);
            }
            openFileDialog.InitialDirectory = default_path;
            if (create_mode)
            {
                openFileDialog.CheckFileExists = false;
                int i = 1;
                while (Directory.GetFiles(default_path).Contains($"{default_path}\\vault_{i}.vault"))
                    i += 1;
                var t = Directory.GetFiles(default_path);
                openFileDialog.FileName = $"vault_{i}.vault";
            }
            else
                openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = $"{vault_string} (*.vault)|*.vault";
            return openFileDialog;
        }
    }
}
