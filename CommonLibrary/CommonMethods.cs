using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CommonLibrary
{
    public static class CommonMethods
    {
        public static string GetRandomString(int Length = 32)
        {
            string sRandom = string.Empty;
            string sAllChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random r = new Random();
            for (int i = 0; i < Length; i++)
            {
                sRandom += sAllChar.Substring(r.Next(sAllChar.Length - 1), 1);
            }
            return sRandom;
        }

        public static string ImageToBase64(string ImagePath)
        {
            string path = Path.GetFullPath(ImagePath).Replace("~\\", "");
            if (System.IO.File.Exists(path))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(path);
                return Convert.ToBase64String(imageArray);
            }
            return string.Empty;
        }

        /// <summary>
        /// this function used to Resize Image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Image ResizeImage(Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = 0;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(cpy))
                {
                    gr.Clear(Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.DrawImage(img,
                        new Rectangle(0, 0, nnx, nny),
                        new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                }
                return cpy;
            }
        }
    }
}
