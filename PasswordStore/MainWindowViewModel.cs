using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStore
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool telegramWidgetsVisible = false;

        public bool TelegramWidgetsVisible
        {
            get
            {
                return telegramWidgetsVisible;
            }
            set
            {
                telegramWidgetsVisible = value;
                NotifyPropertyChanged("TelegramWidgetsVisible");
            }
        }

        private void NotifyPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
