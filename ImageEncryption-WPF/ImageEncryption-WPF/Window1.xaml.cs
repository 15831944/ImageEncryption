using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageEncryption_WPF
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    this.progressBar1.Dispatcher.BeginInvoke((ThreadStart)delegate { this.progressBar1.Value = i; });
                    Thread.Sleep(100);
                }

            }));
            thread.Start();
        }
    }
}
