using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Block
{
    public class ImageEncryption
    {
        public readonly int BlockWidth;

        public readonly int BlockHeight;

        public ImageEncryption(int blockWidth, int blockHeight)
        {
            BlockWidth = blockWidth;
            BlockHeight = blockHeight;
        }

        public Bitmap CbcEncrypt(Bitmap sourceImage, bool[] key)
        {
            var image = (Bitmap) sourceImage.Clone();
            var blockSize = BlockWidth * BlockHeight;
            EcbEncryptBlock(image, key, 0, 0);

            for (var x = 1; x < image.Width / BlockWidth; x++)
            {
                for (var y = 1; y < image.Height / BlockHeight; y++)
                {
                    var previousBlockAsKey = GetBlockKey(image, x - 1, y - 1);
                    var newKey = new bool[blockSize];

                    for (var i = 0; i < blockSize; i++)
                    {
                        newKey[i] = previousBlockAsKey[i] ^ key[i];
                    }

                    EcbEncryptBlock(image, newKey, x, y);
                }
            }

            return image;
        }

        public Bitmap EcbEncrypt(Bitmap sourceImage, bool[] key)
        {
            var image = (Bitmap) sourceImage.Clone();

            for (var x = 0; x < image.Width / BlockWidth; x++)
            {
                for (var y = 0; y < image.Height / BlockHeight; y++)
                {
                    EcbEncryptBlock(image, key, x, y);
                }
            }

            return image;
        }

        private void EcbEncryptBlock(Bitmap image, bool[] key, int x, int y)
        {
            var block = GetPixelBlock(x, y).ToArray();
            for (var px = 0; px < BlockWidth * BlockHeight; px++)
            {
                if (key[px])
                    FlipPixelColor(image, block[px]);
            }
        }

        private bool[] GetBlockKey(Bitmap image, int x, int y)
        {
            return GetPixelBlock(x, y).Select(px => IsPixelBlack(image, px)).ToArray();
        }

        private IEnumerable<Point> GetPixelBlock(int x, int y)
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

        private void FlipPixelColor(Bitmap image, Point px)
        {
            var newColor = IsPixelBlack(image, px) ? Color.White : Color.Black;
            image.SetPixel(px.X, px.Y, newColor);
        }

        private bool IsPixelBlack(Bitmap image, Point px)
        {
            var color = image.GetPixel(px.X, px.Y);
            return color.R == 0 && color.G == 0 && color.B == 0;
        }
    }
}
