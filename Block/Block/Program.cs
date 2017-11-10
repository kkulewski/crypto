using System;
using System.Drawing;

namespace Block
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = LoadImage(args[0]);

            var key = new[] {false, true, true, true};

            var newImg = Flip4Encryption(key, img);
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

        public static Bitmap Flip4Encryption(bool[] key, Bitmap source)
        {
            var img = (Bitmap) source.Clone();

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
    }
}
