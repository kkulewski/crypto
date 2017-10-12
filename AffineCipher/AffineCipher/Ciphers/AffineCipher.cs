using System;
using System.Collections.Generic;

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
            VerifyKey(key);

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
            // to lowercase for easier comparison
            var plainChars = plain.ToLower().ToCharArray();
            var encryptedChars = encrypted.ToLower().ToCharArray();

            int i = -1;
            int j = i + 1;
            int x1 = 0, x2, y1, y2;
            var offset = 'a';
            int coefficentInversion = 0;

            bool inversionFound = false;
            while (!inversionFound && i + 1 < plainChars.Length)
            {
                i++;
                while (!inversionFound && j + 1 < plainChars.Length)
                {
                    j++;
                    // loop until a valid pair is found and letter i != i+1
                    if (IsALetter(plainChars[i]) && IsALetter(plainChars[j]) && plainChars[i] != plainChars[j])
                    {
                        x1 = plainChars[i] - offset;
                        x2 = plainChars[j] - offset;

                        var coefficent = (x1 - x2 + AlphabetSize) % AlphabetSize;
                        // if inversion of given coefficent exists - break the loop
                        inversionFound = Modulo.GetInversion(coefficent, AlphabetSize, out coefficentInversion);
                    }
                }

                if (!inversionFound)
                {
                    j = 1;
                }
            }

            if (!inversionFound)
            {
                throw new Exception("Key cannot be found.");
            }

            // equations: y1 = a*x1 + b, y2 = a*x2 + b
            y1 = encryptedChars[i] - offset;
            y2 = encryptedChars[j] - offset;

            // ensure mod operations are performed on positive numbers
            var alphabetSizeTimes1000 = AlphabetSize * 1000;
            // solve modulo equation system
            var a = ((y1 - y2) * coefficentInversion + alphabetSizeTimes1000) % AlphabetSize;
            var b = (y1 - ((a * x1 + alphabetSizeTimes1000) % AlphabetSize) + alphabetSizeTimes1000) % AlphabetSize;

            return new Key(a, b);
        }

        public override IEnumerable<Key> GetPossibleKeys()
        {
            var keys = new List<Key>();
            for (int i = 0; i < AlphabetSize; i++)
            {
                if (Modulo.RelativelyPrime(i, AlphabetSize))
                {
                    for (int j = 0; j < AlphabetSize; j++)
                    {
                        keys.Add(new Key(i, j));
                    }
                }
            }

            return keys;
        }

        private bool IsALetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
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
