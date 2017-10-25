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
                    Encrypt(GetKeyBytes());
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

            var input = File.ReadAllText(FileNames.OriginalText, Encoding.ASCII);
            input = input.ToLower().RemoveForbiddenCharacters();

            var preparedText = new StringBuilder();
            while (input.Length > lineLength)
            {
                var line = input.Substring(0, lineLength);
                input = input.Substring(lineLength, input.Length - lineLength);
                preparedText.AppendLine(line);
            }

            File.WriteAllText(FileNames.PreparedText, preparedText.ToString(), Encoding.ASCII);
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

        public static byte[] GetKeyBytes()
        {
            const int alphabetStart = 'a';
            var keyText = File.ReadAllText(FileNames.Key, Encoding.ASCII);
            var keyBytes = Encoding.ASCII.GetBytes(keyText);

            for(int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = (byte)(keyBytes[i] - alphabetStart);
            }

            return keyBytes;
        }

        public static void Encrypt(byte[] keyBytes)
        {
            int lineLength = 16 - 1;

            var inputText = File.ReadAllText(FileNames.PreparedText, Encoding.ASCII);
            var inputBytes = Encoding.ASCII.GetBytes(inputText);

            var outputBytes = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                if (inputBytes[i] != (byte)'\n')
                {
                    outputBytes[i] = (byte) (inputBytes[i] ^ keyBytes[i % lineLength]);
                }
                else
                {
                    outputBytes[i] = inputBytes[i];
                }
            }

            var outputText = Encoding.ASCII.GetString(outputBytes);
            File.WriteAllText(FileNames.EncryptedText, outputText, Encoding.ASCII);

            // debug
            for (int i = 0; i < inputBytes.Length; i++)
            {
                Console.WriteLine("{0:X} => {1:X} => {2:X}",
                    inputBytes[i],
                    outputBytes[i],
                    inputBytes[i] != '\n' ? outputBytes[i] ^ keyBytes[i % lineLength] : outputBytes[i]);
            }
        }
    }
}
