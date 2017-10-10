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
                    chars[i] = (char)((chars[i] - 'a' + key.Addend) % AlphabetSize + 'a');
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = (char)((chars[i] - 'A' + key.Addend) % AlphabetSize + 'A');
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
                    // add alphabetSize to ensure mod on non-negative integers
                    chars[i] = (char)((chars[i] - 'a' - key.Addend + AlphabetSize) % AlphabetSize + 'a');
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = (char)((chars[i] - 'A' - key.Addend + AlphabetSize) % AlphabetSize + 'A');
                }
            }

            return new string(chars);
        }

        public override Key RunCryptoanalysisWithPlain(string plain, string encrypted)
        {
            var plainChars = plain.ToCharArray();
            var encryptedChars = encrypted.ToCharArray();

            int i = 0;
            // loop until a letter is found
            while (!((plainChars[i] >= 'a' && plainChars[i] <= 'z') || (plainChars[i] >= 'A' && plainChars[i] <= 'Z')))
            {
                i++;
            }

            var keyAddend = (encryptedChars[i] - plainChars[i] + AlphabetSize) % AlphabetSize;
            var keyMultiplier = 1;
            return new Key(keyMultiplier, keyAddend);
        }
    }
}
