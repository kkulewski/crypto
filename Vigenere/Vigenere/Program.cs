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
                        //Console.WriteLine(GetKeyLength(FileNames.EncryptedText));
                        FindKey(GetKeyLength(FileNames.EncryptedText), FileNames.EncryptedText);
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

        public static int GetKeyLength(string encryptedFileName)
        {
            var inputText = File.ReadAllText(encryptedFileName, Encoding.ASCII);
            var inputChars = inputText.ToCharArray();
            var occurrences = new int[inputChars.Length];

            for (var i = 1; i < inputChars.Length; i++)
            {
                for (var j = i; j < inputChars.Length; j++)
                {
                    if (inputChars[j - i] == inputChars[j])
                        occurrences[i]++;
                }
            }

            var descendingOccurrences = occurrences.OrderByDescending(x => x);
            var percentile98Average = descendingOccurrences.Take(inputChars.Length / 50).Average();

            var top1 = descendingOccurrences.First();
            var indexOfTop1 = Array.IndexOf(occurrences, top1);

            var keyLength = 0;
            for (var i = 1; i < inputChars.Length; i++)
            {
                if (occurrences[i] >= percentile98Average && indexOfTop1 % i == 0)
                {
                    keyLength = i;
                    break;
                }
            }

            return keyLength;
        }

        public static string FindKey(int keyLength, string encryptedFileName)
        {
            var input = File.ReadAllText(encryptedFileName, Encoding.ASCII);

            var englishFrequency = new[]
            {
                8.167, 1.492, 2.782, 4.253, 12.702, 2.228, 2.015, 6.094, 6.966, 0.153, 0.772, 4.025, 2.406,
                6.749, 7.507, 1.929, 0.095, 5.987, 6.327, 9.056, 2.758, 0.978, 2.360, 0.150, 1.974, 0.974
            };

            var key = new char[keyLength+1];

            // for each index in key
            for (var i = 0; i < keyLength; i++)
            {
                // for each letter in alphabet (each offset)
                var scalarProducts = new double[AlphabetSize];
                for (var k = 0; k < AlphabetSize; k++)
                {
                    // count letter occurrences at index cipherLen % key
                    var letterOccurrences = new int[AlphabetSize];
                    for (var j = 0; j < input.Length; j += keyLength)
                    {
                        var currentLetter = (input[j + i] - 'a' - k + AlphabetSize) % AlphabetSize;
                        letterOccurrences[currentLetter]++;
                    }

                    var lettersSum = letterOccurrences.Sum();
                    var letterFrequency = new double[AlphabetSize];
                    for (var j = 0; j < AlphabetSize; j++)
                    {
                        letterFrequency[j] = ((double)letterOccurrences[j] / lettersSum) * 100;
                    }

                    scalarProducts[k] = GetScalarProduct(englishFrequency, letterFrequency);
                }

                var max = scalarProducts.OrderByDescending(x => x).First();
                key[i] = (char)(Array.IndexOf(scalarProducts, max) + 'a');
            }







            var u = 3;






            //var product = 0.0;
            //var indexOffset = 0;
            //for (var i = 0; i < keyLength - 1; i++)
            //{
            //    var inputOccurrences = new int[AlphabetSize];
            //    for (var j = 0; j < input.Length; j++)
            //    {
            //        var currentLetter = input[i % keyLength] - 'a';
            //        inputOccurrences[currentLetter]++;
            //    }

            //    var totalOccurrences = inputOccurrences.Sum();
                
            //    var inputFrequency = new double[AlphabetSize];
            //    for (var j = 0; j < AlphabetSize; j++)
            //    {
            //        inputFrequency[j] = (double) inputOccurrences[j] / totalOccurrences;
            //    }

            //    var newProduct = GetScalarProduct(inputFrequency, englishFrequency);
            //    if (newProduct > product)
            //    {
            //        indexOffset = i;
            //        product = newProduct;
            //    }


            //}
            var a = 5;
            return "aaa";
        }

        public static double GetScalarProduct(double[] vector1, double[] vector2)
        {
            var scalarProduct = 0.0;
            for (var i = 0; i < vector1.Length; i++)
            {
                scalarProduct += vector1[i] * vector2[i];
            }

            return scalarProduct;
        }
    }
}
