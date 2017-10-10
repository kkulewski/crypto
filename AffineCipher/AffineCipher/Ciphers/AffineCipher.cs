using System;

namespace AffineCipher.Ciphers
{
    class AffineCipher : Cipher
    {
        public override string Encrypt(string input, Key key)
        {
            VerifyKey(key);

            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    var offset = 'a' - 1;
                    var c = ((chars[i] - offset) * key.Multiplier + key.Addend) % AlphabetSize + offset;
                    chars[i] = (char) c;
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    var offset = 'A' - 1;
                    var c = ((chars[i] - offset) * key.Multiplier + key.Addend) % AlphabetSize + offset;
                    chars[i] = (char) c;
                }
            }

            return new string(chars);
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
