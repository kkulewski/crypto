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
                        // encrypt
                        break;

                    case "-d":
                        // decrypt
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
    }
}
