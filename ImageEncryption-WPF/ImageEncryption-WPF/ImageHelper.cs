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
        private static readonly int Key = 134;

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
            var r = color.R + Key;
            if (r > 255)
            {
                r = r - 256;
            }
            var g = color.G - Key;
            if (g < 0)
            {
                g = 256 + g;
            }
            var b = color.B + Key;
            if (b > 255)
            {
                b = b - 256;
            }
            return Color.FromArgb(a, r, g, b); ;
        }
    }
}
