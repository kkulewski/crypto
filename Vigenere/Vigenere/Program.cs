using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigenere
{
    static class Program
    {
        public static int AlphabetSize = 'z' - 'a' + 1;

        static void Main(string[] args)
        {
            const string parameterHelp = " Try:" +
                                         "\n-p -- prepare text" +
                                         "\n-e -- encrypt text" +
                                         "\n-d -- decrypt text" +
                                         "\n-k -- cryptoanalysis";

            if (args.Length != 1 || args[0] == null)
            {
                Console.WriteLine("Missing action parameter!" + parameterHelp);
                return;
            }

            try
            {
                switch (args[0])
                {
                    case "-p":
                        PrepareText(FileNames.OriginalText, FileNames.PreparedText);
                        break;

                    case "-e":
                        EncryptFile(FileNames.Key, FileNames.PreparedText, FileNames.EncryptedText);
                        break;

                    case "-d":
                        EncryptFile(FileNames.Key, FileNames.EncryptedText, FileNames.DecryptedText, inverse: true);
                        break;

                    case "-k":
                        // cryptoanalysis
                        break;

                    default:
                        Console.WriteLine("Wrong action parameter!" + parameterHelp);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public static void PrepareText(string inputFileName, string outputFileName)
        {
            var input = File.ReadAllText(inputFileName, Encoding.ASCII);
            var preparedText = input.ToLower().RemoveForbiddenCharacters();
            File.WriteAllText(outputFileName, preparedText, Encoding.ASCII);
        }

        public static string RemoveForbiddenCharacters(this string input)
        {
            var forbiddenChars =
                "~ ` ! @ # $ % ^ & * ( ) _ + 1 2 3 4 5 6 7 8 9 0 - = [ ] { } ; ' \" : , . < > / ? \\ | \n \r \t"
                    .Split(' ');

            foreach (var s in forbiddenChars)
            {
                input = input.Replace(s, "");
            }

            return input.Replace(" ", string.Empty);
        }

        public static void EncryptFile(string keyFileName, string inputFileName, string outputFileName, bool inverse = false)
        {
            var key = File.ReadAllText(keyFileName, Encoding.ASCII);
            var input = File.ReadAllText(inputFileName, Encoding.ASCII);
            var encrypted = Encrypt(key, input, inverse);
            File.WriteAllText(outputFileName, encrypted, Encoding.ASCII);
        }

        public static string Encrypt(string key, string input, bool inverse = false)
        {
            var keyChars = key.ToCharArray();
            var inputChars = input.ToCharArray();
            var outputChars = new char[inputChars.Length];

            const char offset = 'a';
            var sign = inverse ? -1 : 1;

            for (var i = 0; i < input.Length; i++)
            {
                var currentKey = (keyChars[i % keyChars.Length] - offset) * sign;
                var currentCharIndex = inputChars[i] - offset;
                outputChars[i] = (char)(((currentCharIndex + currentKey + AlphabetSize) % AlphabetSize) + offset);
            }

            return new string(outputChars);
        }
    }
}
