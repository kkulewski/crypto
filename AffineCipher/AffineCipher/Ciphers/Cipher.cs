namespace AffineCipher.Ciphers
{
    public abstract class Cipher
    {
        public int AlphabetSize = 'z' - 'a' + 1;

        public abstract string Encrypt(string input, Key key);

        public abstract string Decrypt(string input, Key key);

        public abstract Key RunCryptoanalysisWithPlain(string plain, string encrypted);

        protected bool IsLowercaseLetter(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        protected bool IsUppercaseLetter(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
    }
}
