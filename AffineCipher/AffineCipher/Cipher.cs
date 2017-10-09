namespace AffineCipher
{
    public class Cipher
    {
        public static string EncryptCaesar(string input, int key)
        {
            var alphabetSize = 'z' - 'a' + 1;
            var chars = input.ToCharArray();

            for(int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    chars[i] = (char)((chars[i] - 'a' + key) % alphabetSize + 'a');
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = (char)((chars[i] - 'A' + key) % alphabetSize + 'A');
                }
            }

            return new string(chars);
        }

        public static string DecryptCaesar(string input, int key)
        {
            var alphabetSize = 'z' - 'a' + 1;
            var chars = input.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    // add alphabetSize to ensure mod on non-negative integers
                    chars[i] = (char)((chars[i] - 'a' - key + alphabetSize) % alphabetSize + 'a');
                }

                if (chars[i] >= 'A' && chars[i] <= 'Z')
                {
                    chars[i] = (char)((chars[i] - 'A' - key + alphabetSize) % alphabetSize + 'A');
                }
            }

            return new string(chars);
        }
    }
}
