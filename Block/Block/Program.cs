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
            var newImg = EcbEncrypt(img, key);
            newImg.Save(args[2]);
        }

        public static bool[] LoadKey(string fileName)
        {
            var keyChars = File.ReadAllText(fileName).ToCharArray();
            var key = new List<bool>();

            foreach (var c in keyChars)
            {
                key.Add(c == '1');
            }

            return key.ToArray();
        }

        public static Bitmap LoadImage(string fileName)
        {
            return (Bitmap) Image.FromFile(fileName);
        }
        
        public static Bitmap EcbEncrypt(Bitmap sourceImage, bool[] key)
        {
            var image = (Bitmap)sourceImage.Clone();

            for (var i = 0; i < image.Width / 4; i++)
            {
                for (var j = 0; j < image.Height / 4; j++)
                {
                    EcbEncryptBlock(image, key, i, j);
                }
            }

            return image;
        }

        public static void EcbEncryptBlock(Bitmap image, bool[] key, int i, int j)
        {
            var points = Get16Points(i, j).ToArray();
            for (var k = 0; k < 16; k++)
            {
                if (key[k])
                    FlipPixelColor(image, points[k]);
            }
        }

        public static bool IsPixelBlack(Bitmap img, Point p)
        {
            var px = img.GetPixel(p.X, p.Y);
            return px.R == 0 && px.G == 0 && px.B == 0;
        }

        public static void FlipPixelColor(Bitmap img, Point p)
        {
            var newColor = IsPixelBlack(img, p) ? Color.White : Color.Black;
            img.SetPixel(p.X, p.Y, newColor);
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
