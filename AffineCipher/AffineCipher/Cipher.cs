namespace AffineCipher
{
    public abstract class Cipher
    {
        public const int AlphabetSize = 'z' - 'a' + 1;

        public abstract string Encrypt(string input, Key key);

        public abstract string Decrypt(string input, Key key);

        public abstract Key RunCryptoanalysisWithPlain(string plain, string encrypted);
    }
}
