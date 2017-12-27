using System;
using System.IO;
using System.Numerics;

namespace ElGamal
{
    class Program
    {
        public const int RandomExponentMax = 100;

        static void Main(string[] args)
        {
            const string parameterHelp = " Try:" +
                                         "\n-k -- generate keys" +
                                         "\n-e -- encrypt text" +
                                         "\n-d -- decrypt text" +
                                         "\n-s -- generate signature" +
                                         "\n-v -- verify signature";

            if (args.Length != 1 || args[0] == null)
            {
                Console.WriteLine("Missing action parameter!" + parameterHelp);
                return;
            }

            try
            {
                switch (args[0])
                {
                    case "-k":
                        GenerateKeys(FileNames.Generator);
                        break;

                    case "-e":
                        Encrypt(FileNames.PublicKey, FileNames.PlainText);
                        break;

                    case "-d":
                        break;

                    case "-s":
                        break;

                    case "-v":
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

        public static void GenerateKeys(string generatorFileName)
        {
            var lines = File.ReadAllLines(generatorFileName);
            var prime = BigInteger.Parse(lines[0]);
            var generator = BigInteger.Parse(lines[1]);

            var random = new Random();
            var exponent = random.Next(1, RandomExponentMax);
            var power = BigInteger.ModPow(generator, exponent, prime);

            var output = prime + Environment.NewLine + generator + Environment.NewLine;

            var privateKey = output + exponent + Environment.NewLine;
            File.WriteAllText(FileNames.PrivateKey, privateKey);

            var publicKey = output + power + Environment.NewLine;
            File.WriteAllText(FileNames.PublicKey, publicKey);
        }

        public static void Encrypt(string publicKeyFileName, string messageFileName)
        {
            var publicKeyLines = File.ReadAllLines(publicKeyFileName);
            var messageLines = File.ReadAllLines(messageFileName);
            
            var random = new Random();
            var k = random.Next(1, RandomExponentMax);

            var message = BigInteger.Parse(messageLines[0]);
            var prime = BigInteger.Parse(publicKeyLines[0]);

            if (message >= prime)
                throw new Exception("m < p condition not met");

            var generator = BigInteger.Parse(publicKeyLines[1]);
            var gk = BigInteger.ModPow(generator, k, prime);

            var publicKey = BigInteger.Parse(publicKeyLines[2]);
            var bk = BigInteger.ModPow(publicKey, k, prime);
            var encryptedMessage = (message * bk) % prime;

            var output = gk + Environment.NewLine + encryptedMessage + Environment.NewLine;
            File.WriteAllText(FileNames.EncryptedText, output);
        }
    }
}
