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
                    PrepareText(FileNames.OriginalText, FileNames.PreparedText);
                    break;

                case "-e":
                    Encrypt(GetKeyBytes(FileNames.Key), FileNames.PreparedText, FileNames.EncryptedText);
                    break;

                case "-d":
                    Encrypt(GetKeyBytes(FileNames.Key), FileNames.EncryptedText, FileNames.DecryptedText);
                    break;

                case "-k":
                    Cryptoanalysis(FileNames.EncryptedText, FileNames.DecryptedText, FileNames.CrackedKey);
                    break;

                default:
                    Console.WriteLine("Wrong action parameter!" + parameterHelp);
                    break;
            }
        }

        public static void PrepareText(string inputFileName, string outputFileName)
        {
            int lineLength = 16;

            var input = File.ReadAllText(inputFileName, Encoding.ASCII);
            input = input.ToLower().RemoveForbiddenCharacters();

            var preparedText = new StringBuilder();
            while (input.Length > lineLength)
            {
                var line = input.Substring(0, lineLength);
                input = input.Substring(lineLength, input.Length - lineLength);
                preparedText.Append(line + '\n');
            }

            File.WriteAllText(outputFileName, preparedText.ToString(), Encoding.ASCII);
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

        public static byte[] GetKeyBytes(string inputFileName)
        {
            var keyText = File.ReadAllText(inputFileName, Encoding.ASCII);
            return Encoding.ASCII.GetBytes(keyText);
        }

        public static void Encrypt(byte[] keyBytes, string inputFileName, string outputFileName)
        {
            int lineLength = 16;

            var inputText = File.ReadAllText(inputFileName, Encoding.ASCII);
            var inputBytes = Encoding.ASCII.GetBytes(inputText);
            var inputLines = inputBytes.Length / (lineLength + 1);

            var outputBytes = new byte[inputBytes.Length];

            for (int i = 0; i < inputLines; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    outputBytes[i * (lineLength + 1) + j] = (byte)(inputBytes[i * (lineLength + 1) + j] ^ keyBytes[j]);
                }

                outputBytes[i * (lineLength + 1) + lineLength] = (byte)'\n';
            }

            var outputText = Encoding.ASCII.GetString(outputBytes);
            File.WriteAllText(outputFileName, outputText, Encoding.ASCII);
        }

        public static void Cryptoanalysis(string inputFileName, string outputFileName, string outputKeyFileName)
        {
            int lineLength = 16;
            var inputText = File.ReadAllText(inputFileName, Encoding.ASCII);
            var inputBytes = Encoding.ASCII.GetBytes(inputText);
            var textLines = inputBytes.Length / (lineLength + 1);
            var keyBytes = new byte[lineLength];

            // ALGORITHM
            // A = inputBytes[i], B = inputBytes[i+1], C = inputBytes[i+2]
            // spaceMask = 0100_0000
            // if A ^ B & spaceMask != 0 => either A is space or B is space
            // so: if B ^ C & spaceMask == 0, then neither B nor C is space => A might be a space
            // now, when we know that A is space => A ^ space == keyByte[i]
            var spaceMask = Convert.ToByte("01000000", 2);


            for (int i = 1; i < textLines; i++)
            {
                for (int j = 0; j < lineLength - 2; j++)
                {
                    // current line offset
                    var cl = i * (lineLength + 1);

                    // first, second and third character to compare (A, B, C)
                    int a = j + 0, b = j + 1, c = j + 2;

                    // abs = (A xor B) & spaceBit
                    byte abs = (byte)((inputBytes[cl + a] ^ inputBytes[cl + b]) & spaceMask);
                    byte acs = (byte)((inputBytes[cl + a] ^ inputBytes[cl + c]) & spaceMask);
                    byte bcs = (byte)((inputBytes[cl + b] ^ inputBytes[cl + c]) & spaceMask);

                    // either A or B is space
                    if (abs != 0)
                    {
                        if (acs != 0 && bcs == 0) // A+C contains space, B+C does not contain space => A is space
                            keyBytes[a] = (byte)(inputBytes[cl + a] ^ (byte)' ');
                        else if (acs == 0 && bcs != 0) // A+C does not contain space, B+C contains space => B is space
                            keyBytes[b] = (byte)(inputBytes[cl + b] ^ (byte)' ');
                    }

                    if (acs != 0)
                    {
                        if (abs != 0 && bcs == 0)
                            keyBytes[a] = (byte)(inputBytes[cl + a] ^ (byte)' ');
                        else if (abs == 0 && bcs != 0)
                            keyBytes[c] = (byte)(inputBytes[cl + c] ^ (byte)' ');
                    }

                    if (bcs != 0)
                    {
                        if (abs != 0 && acs == 0)
                            keyBytes[b] = (byte)(inputBytes[cl + b] ^ (byte)' ');
                        else if (abs == 0 && acs != 0)
                            keyBytes[c] = (byte)(inputBytes[cl + c] ^ (byte)' ');
                    }
                }
            }

            var key = new StringBuilder();
            for (int i = 0; i < lineLength; i++)
            {
                key.Append((char)keyBytes[i]);
            }

            File.WriteAllText(outputKeyFileName, key.ToString(), Encoding.ASCII);
            Encrypt(keyBytes, inputFileName, outputFileName);
        }
    }
}
