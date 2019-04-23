using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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

            progressBar.Visibility = Visibility.Collapsed;
        }

        private string Path = string.Empty;

        private Bitmap bitmap = null;

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
                    if (File.Exists(Path))
                    {
                        bitmap = new Bitmap(Path);
                        UpdateImage();
                    }
                    else
                    {
                        MessageBox.Show(this, "请确认文件是否存在");
                    }
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
                if (bitmap == null)
                {
                    MessageBox.Show(this, "请确认图片已加载");
                }
                //progressBar.Minimum = 1;
                //progressBar.Maximum = bitmap.Width* bitmap.Height;
                progressBar.Visibility = Visibility.Visible;
                ImageHelper imageHelper = new ImageHelper();
                imageHelper.ExportEventHander += ProcessChanged;
                bitmap = imageHelper.Encrypt(bitmap);
                progressBar.Visibility = Visibility.Collapsed;
                UpdateImage();
                MessageBox.Show(this, "加密完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "加密完成:" + ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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
                if (bitmap == null)
                {
                    MessageBox.Show(this, "请确认图片已加载");
                }
                //progressBar.Minimum = 1;
                //progressBar.Maximum = bitmap.Width * bitmap.Height;
                progressBar.Visibility = Visibility.Visible;
                ImageHelper imageHelper = new ImageHelper();
                imageHelper.ExportEventHander += ProcessChanged;
                bitmap = imageHelper.Decrypt(bitmap);
                progressBar.Visibility = Visibility.Collapsed;
                UpdateImage();
                MessageBox.Show(this, "解密完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "解密失败:" + ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
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
                if(bitmap == null)
                {
                    MessageBox.Show(this, "请确认图片已加载");
                }
                SaveFileDialog InvokeDialog = new SaveFileDialog();
                InvokeDialog.Title = "选择导出的文件地址";
                InvokeDialog.Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif";
                InvokeDialog.RestoreDirectory = true;
                InvokeDialog.FileName = System.IO.Path.GetFileName(Path);
                if ((bool)InvokeDialog.ShowDialog(this))
                {
                    var newPath = InvokeDialog.FileName;
                    bitmap.MakeTransparent(System.Drawing.Color.White);
                    bitmap.Save(newPath);
                    MessageBox.Show(this, "导出完成");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "导出失败:" + ex.Message);
            }
        }
        
        /// <summary>
        /// 更新图片展示
        /// </summary>
        private void UpdateImage()
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以
            ms.Close();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();
            image1.Source = image;
        }



        #region 进度条
        
        private void ProcessChanged(object sender, EventArgs e)
        {
            progressBar.Value = (int)sender;
        }

        #endregion
    }
}
