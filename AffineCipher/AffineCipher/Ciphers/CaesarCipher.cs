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
                if (IsLowercaseLetter(chars[i]))
                {
                    chars[i] = EncryptCharacter(chars[i], 'a', key);
                }

                if (IsUppercaseLetter(chars[i]))
                {
                    chars[i] = EncryptCharacter(chars[i], 'A', key);
                }
            }

            return new string(chars);
        }

        public override string Decrypt(string input, Key key)
        {
            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (IsLowercaseLetter(chars[i]))
                {
                    chars[i] = DecryptCharacter(chars[i], 'a', key);
                }

                if (IsUppercaseLetter(chars[i]))
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
                while (!(IsLowercaseLetter(plainChars[i]) || IsUppercaseLetter(plainChars[i])))
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
