using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Block
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = LoadImage(args[0]);
            var key = LoadKey(args[1]);

            //var key = new[]
            //{
            //    true, true, false, false,
            //    false, true, false, true,
            //    true, false, true, false,
            //    false, false, true, true
            //};

            var newImg = Flip16Encryption(img, key);
            newImg.Save(args[2]);
        }

        public static bool[] LoadKey(string fileName)
        {
            var content = File.ReadAllText(fileName);
            var keyBits = content.ToCharArray();

            var key = new List<bool>();
            foreach (var b in keyBits)
            {
                key.Add(b == '1');
            }

            return key.ToArray();
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
            var points = Get16Points(i, j).ToArray();
            for (var k = 0; k < 16; k++)
            {
                if(key[k])
                    FlipColor(points[k], image);
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
