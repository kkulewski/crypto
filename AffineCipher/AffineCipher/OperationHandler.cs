using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AffineCipher.Ciphers;

namespace AffineCipher
{
    public class OperationHandler
    {
        private readonly Cipher _cipher;

        public OperationHandler(Cipher cipher)
        {
            _cipher = cipher;
        }

        public OperationResult Encrypt()
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
                return new OperationResult(false, e.Message);
            }

            return new OperationResult(true, "Encrypted");
        }

        public OperationResult Decrypt()
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
                return new OperationResult(false, e.Message);
            }

            return new OperationResult(true, "Decrypted.");
        }

        public OperationResult RunCryptoanalysisWithPlain()
        {
            try
            {
                var plain = File.ReadAllText(FileNames.SourceFile);
                var encrypted = File.ReadAllText(FileNames.EncryptedFile);

                var key = _cipher.RunCryptoanalysisWithPlain(plain, encrypted);
                string keyString = string.Format("{0} {1}", key.Multiplier, key.Addend);
                File.WriteAllText(FileNames.KeyFile, keyString);

                var decrypted = _cipher.Decrypt(encrypted, key);
                File.WriteAllText(FileNames.DecryptedFile, decrypted);
            }
            catch (Exception e)
            {
                return new OperationResult(false, e.Message);
            }

            return new OperationResult(true, "Found key and decrypted.");
        }

        public OperationResult RunCryptoanalysisWithoutPlain()
        {
            try
            {
                var encrypted = File.ReadAllText(FileNames.EncryptedFile);

                List<string> decrypted = new List<string>();
                for(int i = 1; i < _cipher.AlphabetSize; i++)
                {
                    // TODO: multiplier has to be taken care of
                    decrypted.Add(_cipher.Decrypt(encrypted, new Key(1, i)));
                }

                StringBuilder output = new StringBuilder();
                foreach (var s in decrypted)
                {
                    output.AppendLine(s);
                }

                File.WriteAllText(FileNames.ExtraFile, output.ToString());
            }
            catch (Exception e)
            {
                return new OperationResult(false, e.Message);
            }
            
            return new OperationResult(true, "Encrypted (brute force)");
        }
    }
}
