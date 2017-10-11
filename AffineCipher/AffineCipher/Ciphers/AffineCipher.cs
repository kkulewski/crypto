using System;

namespace AffineCipher.Ciphers
{
    public class AffineCipher : Cipher
    {
        public override string Encrypt(string input, Key key)
        {
            VerifyKey(key);

            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    chars[i] = EncryptCharacter(chars[i], 'a', key);
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = EncryptCharacter(chars[i], 'A', key);
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
                    chars[i] = DecryptCharacter(chars[i], 'a', key);
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = DecryptCharacter(chars[i], 'A', key);
                }
            }

            return new string(chars);
        }

        public override Key RunCryptoanalysisWithPlain(string plain, string encrypted)
        {
            var plainChars = plain.ToCharArray();
            var encryptedChars = encrypted.ToCharArray();

            int i = 0;

            try
            {
                // loop until a letter is found
                while (!((plainChars[i] >= 'a' && plainChars[i] <= 'z') || (plainChars[i] >= 'A' && plainChars[i] <= 'Z')))
                {
                    i++;
                }
            }
            catch (Exception)
            {
                throw new Exception("Key cannot be found.");
            }

            var x1 = plainChars[i] - 'a' + 1;
            // y1 = a*x1 + b
            var y1 = encryptedChars[i] - 'a' + 1;
            
            var x2 = plainChars[i + 1] - 'a' + 1;
            // y2 = a*x2 + b
            var y2 = encryptedChars[i + 1] - 'a' + 1;

            var left = (x1 - x2 + AlphabetSize) % AlphabetSize;

            if (!Modulo.GetInversion(left, AlphabetSize, out int multiplierInversion))
            {
                throw new Exception("Character inversion does not exist");
            }

            var a = ((y1 - y2) * multiplierInversion + 10 * AlphabetSize) % AlphabetSize;

            var axX1 = (a * x1 + AlphabetSize) % AlphabetSize;
            var b = (y1 - axX1 + 10*AlphabetSize) % AlphabetSize;

            return new Key(a, b);
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

        private char EncryptCharacter(int current, int offset, Key key)
        {
            var relative = (current - offset) * key.Multiplier + key.Addend + AlphabetSize;
            return (char)(relative % AlphabetSize + offset);
        }

        private char DecryptCharacter(int current, int offset, Key key)
        {
            if (!Modulo.GetInversion(key.Multiplier, AlphabetSize, out int inversion))
            {
                throw new Exception("Character inversion does not exist.");
            }

            var relative = (current - offset - key.Addend + AlphabetSize) * inversion;
            return (char) (relative % AlphabetSize + offset);
        }
    }
}
