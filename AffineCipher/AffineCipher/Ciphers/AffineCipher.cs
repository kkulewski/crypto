using System;

namespace AffineCipher.Ciphers
{
    class AffineCipher : Cipher
    {
        public override string Encrypt(string input, Key key)
        {
            throw new System.NotImplementedException();
        }

        public override string Decrypt(string input, Key key)
        {
            throw new System.NotImplementedException();
        }

        public override Key RunCryptoanalysisWithPlain(string plain, string encrypted)
        {
            throw new System.NotImplementedException();
        }

        private void VerifyKey(Key key)
        {
            if (!AreRelativelyPrime(key.Multiplier, AlphabetSize))
            {
                var message = string.Format("Key multiplier ({0}) is not relatively prime to alphabet length ({1})",
                    key.Multiplier, AlphabetSize);
                throw new Exception(message);
            }
        }

        private bool AreRelativelyPrime(int a, int b)
        {
            return GreatestCommonDivisor(a, b) == 1;
        }

        private int GreatestCommonDivisor(int a, int b)
        {
            while (b > 0)
            {
                var x = a % b;
                a = b;
                b = x;
            }
            return a;
        }
    }
}
