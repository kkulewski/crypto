namespace AffineCipher
{
    public class CaesarCipher
    {
        public const int AlphabetSize = 'z' - 'a' + 1;

        public string Encrypt(string input, int key)
        {
            var chars = input.ToCharArray();
            for(int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    chars[i] = (char)((chars[i] - 'a' + key) % AlphabetSize + 'a');
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = (char)((chars[i] - 'A' + key) % AlphabetSize + 'A');
                }
            }

            return new string(chars);
        }

        public string Decrypt(string input, int key)
        {
            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    // add alphabetSize to ensure mod on non-negative integers
                    chars[i] = (char)((chars[i] - 'a' - key + AlphabetSize) % AlphabetSize + 'a');
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = (char)((chars[i] - 'A' - key + AlphabetSize) % AlphabetSize + 'A');
                }
            }

            return new string(chars);
        }

        public int RunCryptoanalysisWithPlain(string plain, string encrypted)
        {
            var plainChars = plain.ToCharArray();
            var encryptedChars = encrypted.ToCharArray();

            int i = 0;
            // loop until a letter is found
            while (!((plainChars[i] >= 'a' && plainChars[i] <= 'z') || (plainChars[i] >= 'A' && plainChars[i] <= 'Z')))
            {
                i++;
            }

            return (encryptedChars[i] - plainChars[i] + AlphabetSize) % AlphabetSize;
        }
    }
}
