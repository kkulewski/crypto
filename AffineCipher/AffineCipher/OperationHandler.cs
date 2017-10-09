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
                var key = File.ReadAllText(FileNames.KeyFile);

                string[] keySplitted = key.Split(' ');
                var keyMultiplier = int.Parse(keySplitted[0]);
                var keyAddend = int.Parse(keySplitted[1]);

                var encrypted = _cipher.Encrypt(input, keyAddend, keyMultiplier);
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
    }
}
