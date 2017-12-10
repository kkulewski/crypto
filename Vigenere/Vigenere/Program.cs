using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Vigenere
{
    static class Program
    {
        private static int AlphabetSize = 'z' - 'a' + 1;

        private const char FirstLetterOffset = 'a';

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
                        Cryptoanalysis(FileNames.EncryptedText, FileNames.CrackedKey);
                        EncryptFile(FileNames.CrackedKey, FileNames.EncryptedText, FileNames.CrackedText, inverse: true);
                        break;

                    default:
                        Console.WriteLine("Wrong action parameter!" + parameterHelp);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void PrepareText(string inputFileName, string outputFileName)
        {
            var input = File.ReadAllText(inputFileName, Encoding.ASCII);
            var preparedText = input.ToLower().RemoveForbiddenCharacters();
            File.WriteAllText(outputFileName, preparedText, Encoding.ASCII);
        }

        private static string RemoveForbiddenCharacters(this string input)
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

        private static string Encrypt(string key, string input, bool inverse = false)
        {
            var keyChars = key.ToCharArray();
            var inputChars = input.ToCharArray();
            var outputChars = new char[inputChars.Length];
            var sign = inverse ? -1 : 1;

            for (var i = 0; i < input.Length; i++)
            {
                var currentKey = (keyChars[i % keyChars.Length] - FirstLetterOffset) * sign;
                var currentCharIndex = inputChars[i] - FirstLetterOffset;
                outputChars[i] = (char)(((currentCharIndex + currentKey + AlphabetSize) % AlphabetSize) + FirstLetterOffset);
            }

            return new string(outputChars);
        }

        public static void Cryptoanalysis(string encryptedFileName, string crackedKeyFileName)
        {
            var encryptedText = File.ReadAllText(encryptedFileName, Encoding.ASCII);
            var keyLength = GetKeyLength(encryptedText);
            var keyFound = FindKey(keyLength, encryptedText);
            File.WriteAllText(crackedKeyFileName, keyFound, Encoding.ASCII);
        }

        private static int GetKeyLength(string encryptedText)
        {
            var inputChars = encryptedText.ToCharArray();
            var occurrences = new int[inputChars.Length];

            // count matching characters in input with applied offsets
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
                // if length is both in 98 percentile and top1 is its multiply - possible key length found
                if (occurrences[i] >= percentile98Average && indexOfTop1 % i == 0)
                {
                    keyLength = i;
                    break;
                }
            }

            return keyLength;
        }

        private static string FindKey(int keyLength, string encryptedText)
        {

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
                    for (var j = 0; j < encryptedText.Length - keyLength; j += keyLength)
                    {
                        // skip last iteration (input.Length - keyLength) to
                        // avoid i+j getting out of input array bounds

                        // decrypt characters at position [i + j*keyLength] with key k
                        // if key is correct - frequencies will match english alphabet
                        // alphabet size is added to avoid modulo on negative numbers
                        var currentLetter = (encryptedText[j + i] - FirstLetterOffset - k + AlphabetSize) % AlphabetSize;
                        letterOccurrences[currentLetter]++;
                    }

                    var lettersSum = letterOccurrences.Sum();
                    var letterFrequency = new double[AlphabetSize];
                    for (var j = 0; j < AlphabetSize; j++)
                    {
                        letterFrequency[j] = ((double)letterOccurrences[j] / lettersSum) * 100;
                    }

                    // get scalar product for each key k
                    scalarProducts[k] = GetScalarProduct(englishFrequency, letterFrequency);
                }

                // highest product indicates that letters decrypted with this key (array index)
                // have similar frequency to english alphabet
                var maxProduct = scalarProducts.OrderByDescending(x => x).First();
                var correctKey = Array.IndexOf(scalarProducts, maxProduct);
                key[i] = (char)(correctKey + FirstLetterOffset);
            }

            return new string(key);
        }

        private static double GetScalarProduct(double[] vector1, double[] vector2)
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
