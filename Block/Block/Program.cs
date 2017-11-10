using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Block
{
    class Program
    {
        public const int BlockWidth = 4;
        public const int BlockHeight = 4;

        public const string PlainFileName = "plain.bmp";
        public const string KeyFileName = "key.txt";
        public const string EcbEncryptedFileName = "ecb_crypto.bmp";

        static void Main(string[] args)
        {
            var img = LoadImage(PlainFileName);
            var key = LoadKey(KeyFileName);
            EcbEncrypt(img, key).Save(EcbEncryptedFileName);
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

            for (var x = 0; x < image.Width / BlockWidth; x++)
            {
                for (var y = 0; y < image.Height / BlockHeight; y++)
                {
                    EcbEncryptBlock(image, key, x, y);
                }
            }

            return image;
        }

        public static void EcbEncryptBlock(Bitmap image, bool[] key, int x, int y)
        {
            var block = GetPixelBlock(x, y).ToArray();
            for (var px = 0; px < BlockWidth * BlockHeight; px++)
            {
                if (key[px])
                    FlipPixelColor(image, block[px]);
            }
        }

        public static IEnumerable<Point> GetPixelBlock(int x, int y)
        {
            var block = new List<Point>();
            for (var k = 0; k < BlockWidth; k++)
            {
                for (var n = 0; n < BlockHeight; n++)
                {
                    block.Add(new Point(x * BlockWidth + k, y * BlockHeight + n));
                }
            }

            return block;
        }

        public static void FlipPixelColor(Bitmap image, Point px)
        {
            var newColor = IsPixelBlack(image, px) ? Color.White : Color.Black;
            image.SetPixel(px.X, px.Y, newColor);
        }

        public static bool IsPixelBlack(Bitmap image, Point px)
        {
            var color = image.GetPixel(px.X, px.Y);
            return color.R == 0 && color.G == 0 && color.B == 0;
        }
    }
}
