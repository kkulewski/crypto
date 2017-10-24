using System;
using System.IO;
using System.Text;

namespace OTP
{
    static class Program
    {
        static void Main(string[] args)
        {
            const string parameterHelp = " Try:" +
                                           "\n-p -- prepare text" +
                                           "\n-e -- encrypt text" +
                                           "\n-k -- cryptoanalysis";

            if (args.Length != 1 || args[0] == null)
            {
                Console.WriteLine("Missing action parameter!" + parameterHelp);
                return;
            }

            switch (args[0])
            {
                case "-p":
                    PrepareText();
                    break;

                case "-e":
                    //Encryp();
                    break;

                case "-k":
                    //Cryptoanalysis();
                    break;

                default:
                    Console.WriteLine("Wrong action parameter!" + parameterHelp);
                    break;
            }
        }

        public static void PrepareText()
        {
            int lineLength = 16 - 1;

            var input = File.ReadAllText(FileNames.OriginalText);
            input = input.ToLower().RemoveForbiddenCharacters();

            var preparedText = new StringBuilder();
            while (input.Length > lineLength)
            {
                var line = input.Substring(0, lineLength);
                input = input.Substring(lineLength, input.Length - lineLength);
                preparedText.AppendLine(line);
            }

            File.WriteAllText(FileNames.PreparedText, preparedText.ToString());
        }

        public static string RemoveForbiddenCharacters(this string input)
        {
            var forbiddenChars =
                "! @ # $ % ^ & * ( ) _ + 1 2 3 4 5 6 7 8 9 0 - = [ ] { } ; ' : , . < > / ? \\ |"
                    .Split(' ');

            foreach (var s in forbiddenChars)
            {
                input = input.Replace(s, "");
            }

            return input;
        }
    }
}
