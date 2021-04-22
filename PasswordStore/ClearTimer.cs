using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;

namespace PasswordStore
{
    class ClearTimer
    {
        private Timer timer;
        private TextBlock textBlock;
        public ClearTimer(double interval, TextBlock textBlock, string text)
        {
            timer = new Timer(interval);
            textBlock.Dispatcher.Invoke(new Action(() => { textBlock.Text = text; }));
            timer.Enabled = true;
            this.textBlock = textBlock;
            timer.Elapsed += OnClear;
        }
        private void OnClear(Object source, ElapsedEventArgs e)
        {
            textBlock.Dispatcher.Invoke(new Action(() => { textBlock.Text = ""; }));
        }
    }
}
