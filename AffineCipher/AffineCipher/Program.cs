using System;

namespace AffineCipher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == null)
            {
                Console.WriteLine("No cipher method selected!");
                return;
            }

            if (args[1] == null)
            {
                Console.WriteLine("No action selected!");
                return;
            }

            Cipher cipher;
            var cipherType = args[0];
            var action = args[1];

            switch (cipherType)
            {
                case "-c":
                    cipher = new CaesarCipher();
                    break;

                case "-a":
                    cipher = new AffineCipher();
                    break;

                default:
                    PrintError("Unknown cipher type.");
                    return;
            }

            var fileOperator = new OperationHandler(cipher);

            string result;
            switch (action)
            {
                case "-e":
                    if (fileOperator.Encrypt(out result))
                        PrintSuccess(result);
                    else
                        PrintError(result);
                    break;

                case "-d":
                    if (fileOperator.Decrypt(out result))
                        PrintSuccess(result);
                    else
                        PrintError(result);
                    break;

                case "-j":
                    if (fileOperator.RunCryptoanalysisWithPlain(out result))
                        PrintSuccess(result);
                    else
                        PrintError(result);
                    break;

                case "-k":
                    if (fileOperator.RunCryptoanalysisWithoutPlain(out result))
                        PrintSuccess(result);
                    else
                        PrintError(result);
                    break;

                default:
                    PrintError("Unknown action.");
                    return;
            }

        }

        public static void PrintSuccess(string message)
        {
            Console.WriteLine("## SUCCESS: " + message);
        }

        public static void PrintError(string message)
        {
            Console.WriteLine("## ERROR: " + message);
        }
    }
}
