using System;

namespace AffineCipher.Ciphers
{
    public class CaesarCipher : Cipher
    {
        public override string Encrypt(string input, Key key)
        {
            var chars = input.ToCharArray();
            for(int i = 0; i < chars.Length; i++)
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
            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    chars[i] = DecryptCharacter(chars[i], 'a' - 1, key);
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = DecryptCharacter(chars[i], 'A' - 1, key);
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

            var keyAddend = (encryptedChars[i] - plainChars[i] + AlphabetSize) % AlphabetSize;
            var keyMultiplier = 1;
            return new Key(keyMultiplier, keyAddend);
        }

        private char EncryptCharacter(int current, int offset, Key key)
        {
            return (char)((current - offset + key.Addend) % AlphabetSize + offset);
        }

        private char DecryptCharacter(int current, int offset, Key key)
        {
            // add alphabetSize to ensure mod on non-negative integers
            return (char)((current - offset - key.Addend + AlphabetSize) % AlphabetSize + offset);
        }
    }
}
