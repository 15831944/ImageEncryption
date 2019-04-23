using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace ImageEncryption_WPF
{
    class ImageHelper
    {
        /// <summary>
        /// 加密数值
        /// </summary>
        private static readonly int KeyR = 134;

        /// <summary>
        /// 加密数值
        /// </summary>
        private static readonly int KeyG = 34;

        /// <summary>
        /// 加密数值
        /// </summary>
        private static readonly int KeyB = 200;

        /// <summary>
        /// 图片加密
        /// </summary>
        /// <param name="filePath">源文件</param>
        /// <param name="savePath">保存为文件名称</param>
        public static void EncryptFile(string filePath, string savePath)
        {
            Bitmap bitmap = new Bitmap(filePath);
            Bitmap newImg = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bitmap.GetPixel(i, j);
                    newImg.SetPixel(i, j, PixelEncryption(color));
                }
            }

            newImg.MakeTransparent(Color.White);
            newImg.Save(savePath);
        }

        /// <summary>
        /// 颜色加密
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Color PixelEncryption(Color color)
        {
            var a = color.A;
            var r = color.R + KeyR;
            var g = color.G + KeyG;
            var b = color.B + KeyB;
            if (r > 255)
            {
                r = r - 256;
            }
            if (g > 255)
            {
                g = g - 256;
            }
            if (b > 255)
            {
                b = b - 256;
            }
            return Color.FromArgb(a, r, g, b);
        }
    }
}
