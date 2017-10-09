namespace AffineCipher
{
    class Cipher
    {
        public string CipherCaesar(string input, int key)
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

            return chars.ToString();
        }
    }
}
