using System;
using System.IO;
using System.Numerics;

namespace ElGamal
{
    class Program
    {
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
            var exponent = random.Next(1, 100);
            var power = BigInteger.ModPow(generator, exponent, prime);

            var output = prime + Environment.NewLine + generator + Environment.NewLine;

            var privateKey = output + exponent + Environment.NewLine;
            File.WriteAllText(FileNames.PrivateKey, privateKey);

            var publicKey = output + power + Environment.NewLine;
            File.WriteAllText(FileNames.PublicKey, publicKey);
        }
    }
}
