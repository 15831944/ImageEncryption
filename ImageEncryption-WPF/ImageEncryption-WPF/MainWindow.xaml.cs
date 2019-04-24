using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

            imageHelper.ExportEventHander += ProcessChanged;
            imageHelper.EndEventHander += EncryptDecryptEnd;

        }

        private string Path = string.Empty;

        private Bitmap bitmap = null;
        private ImageHelper imageHelper = new ImageHelper();

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

        #region 加解密
        /// <summary>加密</summary>
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

                progressBar.Visibility = Visibility.Visible;
                UpdateButton(false);
                //MemoryStream ms = new MemoryStream();
                //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                //byte[] bytes = ms.GetBuffer();
                //ms.Close();
                Thread thread = new Thread(new ParameterizedThreadStart(imageHelper.Encrypt));
                thread.Start(bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "加密失败:" + ex.Message);
            }
        }

        /// <summary>解密</summary>
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

                progressBar.Visibility = Visibility.Visible;
                UpdateButton(false);
                Thread thread = new Thread(new ParameterizedThreadStart(imageHelper.Decrypt));
                thread.Start(bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "解密失败:" + ex.Message);
            }
        }


        #endregion

        /// <summary>导出文件</summary>
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

        #region 界面状态变化
        /// <summary>更新图片展示</summary>
        private void UpdateImage()
        {
            this.image1.Dispatcher.BeginInvoke((ThreadStart)delegate
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
            });
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        /// <param name="isEnabled"></param>
        private void UpdateButton(bool isEnabled)
        {

            this.button1.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                button1.IsEnabled = isEnabled;
            });

            this.button2.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                button2.IsEnabled = isEnabled;
            });

            this.button3.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                button3.IsEnabled = isEnabled;
            });

            this.button4.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                button4.IsEnabled = isEnabled;
            });
        }

        #endregion

        #region 进度

        private void ProcessChanged(object sender, EventArgs e)
        {
            //Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    progressBar.Value = (int)sender;
            //}));
            var num = (int)sender;
            this.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { this.progressBar.Value = num; });
        }

        /// <summary>
        /// 加解密结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EncryptDecryptEnd(object sender, EventArgs e)
        {
            try
            {
                if ((bool)sender)
                {
                    bitmap = Model.Instance.Bitmap;
                }
                this.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate
                {
                    progressBar.Visibility = Visibility.Collapsed;
                    progressBar.Value = 0;
                });
                UpdateImage();
                UpdateButton(true);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
