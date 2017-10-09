namespace AffineCipher
{
    public abstract class Cipher
    {
        public const int AlphabetSize = 'z' - 'a' + 1;

        public abstract string Encrypt(string input, int keyAddend, int keyMultiplier = 1);

        public abstract string Decrypt(string input, int keyAddend, int keyMultiplier = 1);

        public abstract int RunCryptoanalysisWithPlain(string plain, string encrypted);
    }
}
