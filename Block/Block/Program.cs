using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Block
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = LoadImage(args[0]);

            var key = new[]
            {
                true, true, false, false,
                false, true, false, true,
                true, false, true, false,
                false, false, true, true
            };

            var newImg = Flip16Encryption(img, key);
            newImg.Save(args[1]);
        }

        public static Bitmap LoadImage(string fileName)
        {
            return (Bitmap) Image.FromFile(fileName);
        }

        public static bool IsPixelBlack(Point p, Bitmap img)
        {
            var px = img.GetPixel(p.X, p.Y);
            return px.R == 0 && px.G == 0 && px.B == 0;
        }

        public static void FlipColor(Point p, Bitmap img)
        {
            img.SetPixel(p.X, p.Y, IsPixelBlack(p, img) ? Color.White : Color.Black);
        }

        public static Bitmap Flip4Encryption(Bitmap sourceImage, bool[] key)
        {
            var img = (Bitmap) sourceImage.Clone();

            for (var i = 0; i < img.Width / 2; i++)
            {
                for (var j = 0; j < img.Height / 2; j++)
                {
                    var p1 = new Point(i * 2, j * 2);
                    var p2 = new Point(i * 2, j * 2 + 1);
                    var p3 = new Point(i * 2 + 1, j * 2);
                    var p4 = new Point(i * 2 + 1, j * 2 + 1);

                    var points = new Point[] { p1, p2, p3, p4 };
                    foreach (var p in points)
                    {
                        if (key[i % 4])
                            FlipColor(p, img);
                    }
                }
            }

            return img;
        }

        public static Bitmap Flip16Encryption(Bitmap sourceImage, bool[] key)
        {
            var image = (Bitmap)sourceImage.Clone();

            for (var i = 0; i < image.Width / 4; i++)
            {
                for (var j = 0; j < image.Height / 4; j++)
                {
                    Flip16EncryptBlock(image, key, i, j);
                }
            }

            return image;
        }

        public static void Flip16EncryptBlock(Bitmap image, bool[] key, int i, int j)
        {
            var points = Get16Points(i, j);
            foreach (var point in points)
            {
                if (key[i % 16])
                    FlipColor(point, image);
            }
        }

        public static IEnumerable<Point> Get16Points(int i, int j)
        {
            var points = new List<Point>();
            for (var k = 0; k < 4; k++)
            {
                for (var n = 0; n < 4; n++)
                {
                    points.Add(new Point(i * 4 + k, j * 4 + n));
                }
            }

            return points;
        }
    }
}
