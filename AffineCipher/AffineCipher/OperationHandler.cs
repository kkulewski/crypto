using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffineCipher
{
    public class OperationHandler
    {
        private readonly Cipher _cipher;

        public OperationHandler(Cipher cipher)
        {
            _cipher = cipher;
        }

        public bool Encrypt(out string result)
        {
            try
            {
                var input = File.ReadAllText(FileNames.SourceFile);
                var keyInput = File.ReadAllText(FileNames.KeyFile);

                string[] keySplitted = keyInput.Split(' ');
                var multiplier = int.Parse(keySplitted[0]);
                var addend = int.Parse(keySplitted[1]);
                var key = new Key(multiplier, addend);

                var encrypted = _cipher.Encrypt(input, key);
                File.WriteAllText(FileNames.EncryptedFile, encrypted);
            }
            catch (Exception e)
            {
                result = e.Message;
                return false;
            }

            result = "Encrypted.";
            return true;
        }

        public bool Decrypt(out string result)
        {
            try
            {
                var input = File.ReadAllText(FileNames.EncryptedFile);
                var keyInput = File.ReadAllText(FileNames.KeyFile);

                string[] keySplitted = keyInput.Split(' ');
                var multiplier = int.Parse(keySplitted[0]);
                var addend = int.Parse(keySplitted[1]);
                var key = new Key(multiplier, addend);

                var decrypted = _cipher.Decrypt(input, key);
                File.WriteAllText(FileNames.DecryptedFile, decrypted);
            }
            catch (Exception e)
            {
                result = e.Message;
                return false;
            }

            result = "Decrypted.";
            return true;
        }
    }
}
