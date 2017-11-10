using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Block
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = LoadImage(args[0]);
            Console.WriteLine("Width : " + img.Width);
            Console.WriteLine("Height: " + img.Height);

            var key = new[] {0, 50, 100, 150};

            for (var i = 0; i < img.Height; i++)
            {
                for (var j = 0; j < img.Width; j++)
                {
                    var color = img.GetPixel(j, i);
                    var offset = key[(j + i) % 4];
                    var newColor = Color.FromArgb(color.A, (color.R + offset) % 255, (color.G + offset) % 255, (color.B + offset) % 255);
                    img.SetPixel(j, i, newColor);
                    //Console.WriteLine(color.R.ToString(), color.G.ToString(), color.B.ToString());
                }
            }

            img.Save(args[1]);
        }

        public static Bitmap LoadImage(string fileName)
        {
            return (Bitmap) Image.FromFile(fileName);
        }
    }
}
