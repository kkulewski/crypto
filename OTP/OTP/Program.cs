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

                case "-d":
                    Decrypt(GetKeyBytes());
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
                preparedText.Append(line + '\n');
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
            var keyText = File.ReadAllText(FileNames.Key, Encoding.ASCII);
            return Encoding.ASCII.GetBytes(keyText);
        }

        public static void Encrypt(byte[] keyBytes)
        {
            int lineLength = 16;

            var inputText = File.ReadAllText(FileNames.PreparedText, Encoding.ASCII);
            var inputBytes = Encoding.ASCII.GetBytes(inputText);

            var outputBytes = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                if (i + 1 % lineLength + 1 == 0 && i + 1 != 0)
                {
                    outputBytes[i] = (byte) '\n';
                }
                else
                {
                    outputBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % lineLength]);
                }
            }

            var outputText = Encoding.ASCII.GetString(outputBytes);
            File.WriteAllText(FileNames.EncryptedText, outputText, Encoding.ASCII);
        }

        public static void Decrypt(byte[] keyBytes)
        {
            int lineLength = 16;

            var inputText = File.ReadAllText(FileNames.EncryptedText, Encoding.ASCII);
            var inputBytes = Encoding.ASCII.GetBytes(inputText);

            var outputBytes = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                if (i+1 % lineLength+1 == 0 && i+1 != 0)
                {
                    outputBytes[i] = (byte)'\n';
                }
                else
                {
                    outputBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % lineLength]);
                }
            }

            var outputText = Encoding.ASCII.GetString(outputBytes);
            File.WriteAllText(FileNames.DecryptedText, outputText, Encoding.ASCII);
        }
    }
}
