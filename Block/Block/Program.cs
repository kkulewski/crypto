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
        public const string CbcEncryptedFileName = "cbc_crypto.bmp";

        static void Main()
        {
            var encryption = new ImageEncryption(BlockWidth, BlockHeight);
            var img = LoadImage(PlainFileName);
            var key = LoadKey(KeyFileName);
            encryption.EcbEncrypt(img, key).Save(EcbEncryptedFileName);
            encryption.CbcEncrypt(img, key).Save(CbcEncryptedFileName);
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
    }
}
