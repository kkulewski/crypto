using System;
using AffineCipher;
using AffineCipher.Ciphers;
using Xunit;

namespace Test
{
    public class AffineCipherTests
    {
        private readonly Cipher _cipher = new AffineCipher.Ciphers.AffineCipher();

        [Fact]
        public void EncryptWithKey17and3()
        {
            var key = new Key(17, 17);
            var input = "test";
            var output = _cipher.Encrypt(input, key);
            
            Assert.Equal("sxbs", output);
        }

        [Fact]
        public void DecryptWithKey17and3()
        {
            var key = new Key(17, 17);
            var input = "sxbs";
            var output = _cipher.Decrypt(input, key);

            Assert.Equal("test", output);
        }

        [Fact]
        public void TwoWayEncryption()
        {
            var key = new Key(17, 17);
            var input = "test";
            var encrypted = _cipher.Encrypt(input, key);
            var decrypted = _cipher.Decrypt(encrypted, key);
            
            Assert.Equal(input, decrypted);
        }

        [Fact]
        public void RunCryptoanalysisWithPlainReturnsCorrectKeyMultiplier()
        {
            var key = new Key(17, 17);
            var input = "test";
            var output = _cipher.Encrypt(input, key);

            Assert.Equal(key.Multiplier, _cipher.RunCryptoanalysisWithPlain(input, output).Multiplier);
        }

        [Fact]
        public void RunCryptoanalysisWithPlainReturnsCorrectKeyAddend()
        {
            var key = new Key(17, 17);
            var input = "test";
            var output = _cipher.Encrypt(input, key);
            
            Assert.Equal(key.Addend, _cipher.RunCryptoanalysisWithPlain(input, output).Addend);
        }
    }
}
