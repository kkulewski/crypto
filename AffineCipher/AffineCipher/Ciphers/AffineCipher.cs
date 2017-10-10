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
                    chars[i] = EncryptCharacter(chars[i], 'a' - 1, key);
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = EncryptCharacter(chars[i], 'A' - 1, key);
                }
            }

            return new string(chars);
        }

        public override string Decrypt(string input, Key key)
        {
            VerifyKey(key);

            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    var offset = 'a' - 1;
                    var currentCharacter = chars[i] - offset;

                    if (!Modulo.GetInversionNaive(key.Multiplier, AlphabetSize, out int inversion))
                    {
                        throw new Exception("Character inversion does not exist.");
                    }

                    var c = ((currentCharacter - key.Addend) * inversion) % AlphabetSize + offset;
                    chars[i] = (char)c;
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    var offset = 'A' - 1;
                    var currentCharacter = chars[i] - offset;

                    if (!Modulo.GetInversionNaive(key.Multiplier, AlphabetSize, out int inversion))
                    {
                        throw new Exception("Character inversion does not exist.");
                    }

                    var c = ((currentCharacter - key.Addend) * inversion) % AlphabetSize + offset;
                    chars[i] = (char)c;
                }
            }

            return new string(chars);
        }

        public override Key RunCryptoanalysisWithPlain(string plain, string encrypted)
        {
            throw new System.NotImplementedException();
        }

        private char EncryptCharacter(int current, int offset, Key key)
        {
            return (char)(((current - offset) * key.Multiplier + key.Addend) % AlphabetSize + offset);
        }

        private void VerifyKey(Key key)
        {
            if (!Modulo.RelativelyPrime(key.Multiplier, AlphabetSize))
            {
                var message = string.Format("Key multiplier ({0}) is not relatively prime to alphabet length ({1})",
                    key.Multiplier, AlphabetSize);
                throw new Exception(message);
            }
        }
    }
}
