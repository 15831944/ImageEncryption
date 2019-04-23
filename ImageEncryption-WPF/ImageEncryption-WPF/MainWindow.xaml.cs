using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageEncryption_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string Path = string.Empty;

        /// <summary>选择文件</summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog InvokeDialog = new OpenFileDialog();
                InvokeDialog.Title = "选择文件";
                //Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*"
                InvokeDialog.Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif";
                InvokeDialog.RestoreDirectory = true;
                if ((bool)InvokeDialog.ShowDialog(this))
                {
                    Path = InvokeDialog.FileName;
                    label1.Content = Path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "选择失败:" + ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Path))
                {
                    MessageBox.Show(this, "请先选择文件");
                }
                if (!File.Exists(Path))
                {
                    MessageBox.Show(this, "请确认文件是否存在");
                }
                SaveFileDialog InvokeDialog = new SaveFileDialog();
                InvokeDialog.Title = "选择导出的文件地址";
                InvokeDialog.Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif";
                InvokeDialog.RestoreDirectory = true;
                InvokeDialog.FileName = System.IO.Path.GetFileName(Path);
                if ((bool)InvokeDialog.ShowDialog(this))
                {
                    var newPath = InvokeDialog.FileName;
                    ImageHelper.EncryptFile2(Path, newPath);
                    MessageBox.Show(this, "导出完成");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "导出失败:" + ex.Message);
            }
        }
    }
}
