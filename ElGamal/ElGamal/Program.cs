﻿using System;
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
                        Decrypt(FileNames.PrivateKey, FileNames.EncryptedText);
                        break;

                    case "-s":
                        Sign(FileNames.PrivateKey, FileNames.MessageText);
                        break;

                    case "-v":
                        var result = VerifySignature(FileNames.PublicKey, FileNames.MessageText, FileNames.Signature);
                        Console.WriteLine(result);
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
            var aliceK = random.Next(1, RandomExponentMax);

            // Alice public key - generator to random number K (g ^ aliceK)
            var alicePublicKey = BigInteger.ModPow(generator, aliceK, prime);

            var output = prime + Environment.NewLine + generator + Environment.NewLine;
            var privateKeyText = output + aliceK + Environment.NewLine;
            var publicKeyText = output + alicePublicKey + Environment.NewLine;
            File.WriteAllText(FileNames.PrivateKey, privateKeyText);
            File.WriteAllText(FileNames.PublicKey, publicKeyText);
        }

        public static void Encrypt(string publicKeyFileName, string messageFileName)
        {
            var publicKeyLines = File.ReadAllLines(publicKeyFileName);
            var messageLines = File.ReadAllLines(messageFileName);

            var message = BigInteger.Parse(messageLines[0]);
            var prime = BigInteger.Parse(publicKeyLines[0]);
            if (message >= prime)
                throw new Exception("m < p condition not met");

            var generator = BigInteger.Parse(publicKeyLines[1]);
            var alicePublicKey = BigInteger.Parse(publicKeyLines[2]);

            var random = new Random();
            var bobK = random.Next(1, RandomExponentMax);

            // Bobs public key - generator to random number K (g ^ bobK)
            var bobPublicKey = BigInteger.ModPow(generator, bobK, prime);

            // common encryption key - (generator ^ aliceK) ^ bobK
            var encryptionKey = BigInteger.ModPow(alicePublicKey, bobK, prime);

            var encryptedMessage = (message * encryptionKey) % prime;
            var output = bobPublicKey + Environment.NewLine + encryptedMessage + Environment.NewLine;
            File.WriteAllText(FileNames.EncryptedText, output);
        }

        public static void Decrypt(string privateKeyFileName, string encryptedMessageFileName)
        {
            var privateKeyLines = File.ReadAllLines(privateKeyFileName);
            var encryptedMessageLines = File.ReadAllLines(encryptedMessageFileName);

            var prime = BigInteger.Parse(privateKeyLines[0]);
            var generator = BigInteger.Parse(privateKeyLines[1]);

            var bobPublicKey = BigInteger.Parse(encryptedMessageLines[0]);
            var bobK = 1;
            while (true)
            {
                // loop till bob K is found
                if (BigInteger.ModPow(generator, bobK, prime) == bobPublicKey)
                    break;

                bobK++;
            }

            // Alice public key => generator ^ aliceK
            var aliceK = BigInteger.Parse(privateKeyLines[2]);
            var alicePublicKey = BigInteger.ModPow(generator, aliceK, prime);

            // encryption key => (generator ^ aliceK) ^ bobK
            var encryptionKey = BigInteger.ModPow(alicePublicKey, bobK, prime);

            var encryptedMessage = BigInteger.Parse(encryptedMessageLines[1]);
            var encryptionKeyInverse = BigInteger.ModPow(encryptionKey, prime - 2, prime);
            var decryptedMessage = (encryptedMessage * encryptionKeyInverse) % prime;

            var output = decryptedMessage + Environment.NewLine;
            File.WriteAllText(FileNames.DecryptedText, output);
        }

        public static void Sign(string privateKeyFileName, string messageFileName)
        {
            var privateKeyLines = File.ReadAllLines(privateKeyFileName);
            var messageLines = File.ReadAllLines(messageFileName);

            var prime = BigInteger.Parse(privateKeyLines[0]);
            var generator = BigInteger.Parse(privateKeyLines[1]);
            var aliceK = BigInteger.Parse(privateKeyLines[2]);
            var message = BigInteger.Parse(messageLines[0]);

            var random = new Random();

            // generate r
            int k;
            while (true)
            {
                var primeInt = prime < int.MaxValue ? (int) prime : int.MaxValue;
                k = random.Next(1, primeInt);
                var relativelyPrime = BigInteger.GreatestCommonDivisor(k, prime - 1) == 1;
                if (relativelyPrime)
                {
                    break;
                }
            }
            var r = BigInteger.ModPow(generator, k, prime);
            
            // generate x
            var kInverse = MultiplicativeInverse(k, prime - 1);
            var mod = prime - 1;
            // ensure modulo operations are performed on positive numbers
            // by adding a "span" (modulo bound * any high-enough multiplier)
            var x1 = message - (aliceK * r) + (mod * 10000);
            var x = (x1 * kInverse) % mod;

            var output = r + Environment.NewLine + x + Environment.NewLine;
            File.WriteAllText(FileNames.Signature, output);
        }

        public static string VerifySignature(string publicKeyFileName, string messageFileName, string signatureFileName)
        {
            var publicKeyLines = File.ReadAllLines(publicKeyFileName);
            var messageLines = File.ReadAllLines(messageFileName);
            var signatureLines = File.ReadAllLines(signatureFileName);

            var prime = BigInteger.Parse(publicKeyLines[0]);
            var generator = BigInteger.Parse(publicKeyLines[1]);
            var publicKey = BigInteger.Parse(publicKeyLines[2]);
            var message = BigInteger.Parse(messageLines[0]);
            var r = BigInteger.Parse(signatureLines[0]);
            var x = BigInteger.Parse(signatureLines[1]);

            var left = BigInteger.ModPow(generator, message, prime);
            var right = (BigInteger.ModPow(publicKey, r, prime) * BigInteger.ModPow(r, x, prime)) % prime;

            string output;
            if (r >= 1 && r < prime && left == right)
            {
                output = "Valid signature";
            }
            else
            {
                output = "Invalid signature";
            }

            File.WriteAllText(FileNames.VerifySignature, output);
            return output;
        }

        public static BigInteger MultiplicativeInverse(BigInteger a, BigInteger m)
        {
            BigInteger x0 = 0;
            BigInteger x1 = 1;
            var m0 = m;

            if (m == 0)
            {
                return 1;
            }

            while (a > 1)
            {
                var q = a / m;
                var t = m;
                m = a % m;
                a = t;

                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0)
            {
                x1 += m0;
            }

            return x1;
        }
    }
}
