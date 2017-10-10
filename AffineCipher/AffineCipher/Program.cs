using System;
using AffineCipher.Ciphers;

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
                    cipher = new Ciphers.AffineCipher();
                    break;

                default:
                    HandleOperationResult(new OperationResult(false, "Unknown cipher type."));
                    return;
            }

            var handler = new OperationHandler(cipher);
            switch (action)
            {
                case "-e":
                    HandleOperationResult(handler.Encrypt());
                    break;

                case "-d":
                    HandleOperationResult(handler.Decrypt());
                    break;

                case "-j":
                    HandleOperationResult(handler.RunCryptoanalysisWithPlain());
                    break;

                case "-k":
                    HandleOperationResult(handler.RunCryptoanalysisWithoutPlain());
                    break;

                default:
                    HandleOperationResult(new OperationResult(false, "Unknown action."));
                    return;
            }

        }

        public static void HandleOperationResult(OperationResult o)
        {
            if (o.Success)
            {
                Console.WriteLine("## SUCCESS: " + o.Message);
            }
            else
            {
                Console.WriteLine("## ERROR: " + o.Message);
            }
        }
    }
}
