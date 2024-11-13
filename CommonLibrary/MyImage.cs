using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CommonLibrary
{
    public class MyImage
    {
        public void CreateImage(string SourcePath, string DestinationPath, ImageFormat Format, bool IsDeleteSourceImage = true)
        {
            Bitmap SourceImage = new Bitmap(SourcePath);
            SourceImage.Save(DestinationPath, Format);
            SourceImage.Dispose();
            DeleteSourceImage(SourcePath, IsDeleteSourceImage);
        }

        public void ResizeImage(string SourcePath, string DestinationPath, int Width, int Height, bool IsDeleteSourceImage = true)
        {
            Bitmap SourceImage = new Bitmap(SourcePath);
            Bitmap DestinationImage = new Bitmap(SourceImage, new Size(Width, Height));
            DestinationImage.Save(DestinationPath);
            SourceImage.Dispose();
            DestinationImage.Dispose();
            DeleteSourceImage(SourcePath, IsDeleteSourceImage);
        }

        public void ResizeImages(string SourcePath, List<string> DestinationPaths, List<int> Widths, List<int> Heights, bool IsDeleteSourceImage = true)
        {
            Bitmap SourceImage = new Bitmap(SourcePath);
            for (int i = 0; i < DestinationPaths.Count; i++)
            {
                Bitmap DestinationImage = new Bitmap(SourceImage, new Size(Widths[i], Heights[i]));
                DestinationImage.Save(DestinationPaths[i]);
                DestinationImage.Dispose();
            }
            SourceImage.Dispose();
            DeleteSourceImage(SourcePath, IsDeleteSourceImage);
        }

        public void ResizeImage(string SourcePath, string DestinationPath, int Percentage, bool IsDeleteSourceImage = true)
        {
            Bitmap SourceImage = new Bitmap(SourcePath);
            int Width = SourceImage.Width * Percentage / 100;
            int Height = SourceImage.Height * Percentage / 100;
            Bitmap DestinationImage = new Bitmap(SourceImage, new Size(Width, Height));
            DestinationImage.Save(DestinationPath);
            SourceImage.Dispose();
            DestinationImage.Dispose();
            DeleteSourceImage(SourcePath, IsDeleteSourceImage);
        }

        public void ResizeImages(string SourcePath, List<string> DestinationPaths, List<int> Percentages, bool IsDeleteSourceImage = true)
        {
            Bitmap SourceImage = new Bitmap(SourcePath);
            for (int i = 0; i < Percentages.Count; i++)
            {
                int Width = SourceImage.Width * Percentages[i] / 100;
                int Height = SourceImage.Height * Percentages[i] / 100;
                Bitmap DestinationImage = new Bitmap(SourceImage, new Size(Width, Height));
                DestinationImage.Save(DestinationPaths[i]);
                DestinationImage.Dispose();
            }
            SourceImage.Dispose();
            DeleteSourceImage(SourcePath, IsDeleteSourceImage);
        }

        public void CreateSquareImage(string SourcePath, string DestinationPath, int Size, bool IsDeleteSourceImage = true)
        {
            Bitmap SourceImage = new Bitmap(SourcePath);
            int Width = 0;
            int Height = 0;
            if (SourceImage.Width > SourceImage.Height)
            {
                Width = Size;
                Height = (int)(SourceImage.Height * Size / SourceImage.Width);
            }
            else
            {
                Height = Size;
                Width = (int)(SourceImage.Width * Size / SourceImage.Height);
            }

            Bitmap DestinationImage = new Bitmap(SourceImage, new Size(Width, Height));
            DestinationImage.Save(DestinationPath);
            SourceImage.Dispose();
            DestinationImage.Dispose();
            DeleteSourceImage(SourcePath, IsDeleteSourceImage);
        }

        public void CreateSquareImages(string SourcePath, List<string> DestinationPaths, List<int> Sizes, bool IsDeleteSourceImage = true)
        {
            Bitmap SourceImage = new Bitmap(SourcePath);
            List<int> Width = new List<int>();
            List<int> Height = new List<int>();

            if (SourceImage.Width > SourceImage.Height)
            {
                for (int i = 0; i < Sizes.Count; i++)
                {
                    Width.Add(Sizes[i]);
                    Height.Add((int)(SourceImage.Height * Sizes[i] / SourceImage.Width));
                }
            }
            else
            {
                for (int i = 0; i < Sizes.Count; i++)
                {
                    Height.Add(Sizes[i]);
                    Width.Add((int)(SourceImage.Width * Sizes[i] / SourceImage.Height));
                }
            }

            for (int i = 0; i < Sizes.Count; i++)
            {
                Bitmap DestinationImage = new Bitmap(SourceImage, new Size(Width[i], Height[i]));
                DestinationImage.Save(DestinationPaths[i]);
                DestinationImage.Dispose();
            }
            SourceImage.Dispose();
            DeleteSourceImage(SourcePath, IsDeleteSourceImage);
        }

        private void DeleteSourceImage(string SourcePath, bool IsDeleteSourceImage = true)
        {
            if (IsDeleteSourceImage)
                File.Delete(SourcePath);
        }
    }

    //public class MyImageEAL
    //{
    //    public string SourcePath { get; set; }
    //    public string DestinationPath { get; set; }
    //    public ImageFormat Format { get; set; }
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //    public int Size { get; set; }
    //    public int Percentage { get; set; }
    //}
}
