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

            Cipher cipher = new CaesarCipher();
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
            }

            switch (action)
            {
                case "-e":
                    Console.WriteLine(cipher.Encrypt("Test", 1));
                    break;

                case "-d":
                    Console.WriteLine(cipher.Decrypt("Uftu", 1));
                    break;

                case "-j":
                    Console.WriteLine(cipher.RunCryptoanalysisWithPlain("Test", "Uftu"));
                    break;
            }

        }
    }
}
