using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ImageEncryption_WPF
{
    /// <summary>
    /// 图片像素修改
    /// </summary>
    class ImageHelper
    {
        /// <summary>
        /// 加密秘钥
        /// </summary>
        private static readonly int Key = 13443521;
        
        /// <summary>
        /// 进度回调事件
        /// </summary>
        public ExportEventHandler ExportEventHander;


        /// <summary>
        /// 完成回调事件
        /// </summary>
        public ExportEventHandler EndEventHander;

        /// <summary>
        /// 图片加密
        /// </summary>
        /// <param name="filePath">源文件</param>
        /// <param name="savePath">保存为文件名称</param>
        public void Encrypt(object obj)
        {
            try
            {
                //Bitmap bitmap = new Bitmap(filePath);
                //Bitmap newImg = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                //byte[] bytelist = (byte[])obj;
                //MemoryStream ms1 = new MemoryStream(bytelist);
                //Bitmap bitmap = (Bitmap)Image.FromStream(ms1);
                //ms1.Close();
                Model.Instance.Bitmap = null;
                Bitmap bitmap = new Bitmap((Bitmap)obj);
                int Width = bitmap.Width;
                int Height = bitmap.Height;
                int expectedCouont = Width * Height;
                Bitmap newImg = new Bitmap(Width, Height);
                //加密
                Stopwatch watch = new Stopwatch();
                watch.Start();
                int[] list = GetRandomIntValues(expectedCouont);
                watch.Stop();
                Console.WriteLine(watch.Elapsed.TotalSeconds);      //记录时间
                int num = -1;
                for (int i = 0; i < list.Length; i++)
                {
                    newImg.SetPixel((i % Width), (i / Width), bitmap.GetPixel((list[i] % Width), (list[i] / Width)));
                    var process = i * 100 / expectedCouont;
                    if (process > num)
                    {
                        num = process;
                        ExportEventHander?.Invoke(num, new EventArgs());
                    }
                }
                //MessageBox.Show("加密完成");
                Model.Instance.Bitmap = newImg;
                EndEventHander?.Invoke(true, new EventArgs());
                //newImg.MakeTransparent(Color.White);
                //newImg.Save(savePath);
                //return newImg;
            }
            catch(Exception ex)
            {
                MessageBox.Show("加密失败:" + ex.Message);
                EndEventHander?.Invoke(false, new EventArgs());
            }
        }

        /// <summary>
        /// 图片解密
        /// </summary>
        /// <param name="obj">Bitmap</param>
        public void Decrypt(object obj)
        {
            try
            {
                Model.Instance.Bitmap = null;
                Bitmap bitmap = new Bitmap((Bitmap)obj);
                int Width = bitmap.Width;
                int Height = bitmap.Height;
                int expectedCouont = Width * Height;
                Bitmap newImg = new Bitmap(Width, Height);
                //加密
                Stopwatch watch = new Stopwatch();
                watch.Start();
                int[] list = GetRandomIntValues(expectedCouont);
                watch.Stop();
                Console.WriteLine(watch.Elapsed.TotalSeconds);      //记录时间
                int num = -1;
                for (int i = 0; i < list.Length; i++)
                {
                    newImg.SetPixel((list[i] % Width), (list[i] / Width), bitmap.GetPixel((i % Width), (i / Width)));
                    var process = i * 100 / expectedCouont;
                    if (process > num)
                    {
                        num = process;
                        ExportEventHander?.Invoke(num, new EventArgs());
                    }
                }
                //MessageBox.Show("解密完成");
                Model.Instance.Bitmap = newImg;
                EndEventHander?.Invoke(true, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show("解密失败:" + ex.Message);
                EndEventHander?.Invoke(false, new EventArgs());
            }
}

        /// <summary>
        /// 获取加密序列
        /// </summary>
        /// <param name="expectedCouont"></param>
        /// <returns></returns>
        private static int[] GetRandomIntValues(int expectedCouont)
        {
            Dictionary<int, int> container = new Dictionary<int, int>(expectedCouont);
            Random r = new Random(Key);
            while (container.Count < expectedCouont)
            {
                int value = r.Next(0, expectedCouont);
                if (!container.ContainsKey(value))
                {
                    container.Add(value, value);
                }
            }
            int[] result = new int[expectedCouont];
            container.Values.CopyTo(result, 0);
            return result;
        }
    }

    
    /// <summary>
    /// 导出进度事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ExportEventHandler(object sender, EventArgs e);
}
