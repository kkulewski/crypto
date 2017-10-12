using System.Linq;
using AffineCipher;
using AffineCipher.Ciphers;
using Xunit;

namespace Test
{
    public class AffineCipherTests
    {
        private readonly Cipher _cipher = new AffineCipher.Ciphers.AffineCipher();

        [Fact]
        public void EncryptWithKey17And17()
        {
            var key = new Key(17, 17);
            var input = "test";
            var output = _cipher.Encrypt(input, key);
            
            Assert.Equal("chlc", output);
        }

        [Fact]
        public void DecryptWithKey17And17()
        {
            var key = new Key(17, 17);
            var input = "chlc";
            var output = _cipher.Decrypt(input, key);

            Assert.Equal("test", output);
        }

        [Fact]
        public void TwoWayEncryption()
        {
            var key = new Key(17, 13);
            var input = "test abcdefghijklmnopqrstuwvxyz 123 . HElLo";
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
            var input = "test asdfghjkl QWERTY zxcv";
            var output = _cipher.Encrypt(input, key);
            
            Assert.Equal(key.Addend, _cipher.RunCryptoanalysisWithPlain(input, output).Addend);
        }

        [Fact]
        public void EncryptDecryptAndCryptoanalysisReturnsCorrectKey()
        {
            var key = new Key(5, 13);
            var input = "qwertyuiop";
            var encrypted = _cipher.Encrypt(input, key);
            var decrypted = _cipher.Decrypt(encrypted, key);
            var foundKey = _cipher.RunCryptoanalysisWithPlain(decrypted, encrypted);
            
            Assert.Equal(input, decrypted);
            Assert.Equal(key.Multiplier, foundKey.Multiplier);
            Assert.Equal(key.Addend, foundKey.Addend);

        }


        [Theory]
        [InlineData("Lorem ipsum")]
        [InlineData("dolor sit amet, consectetur")]
        [InlineData("adipiscing")]
        [InlineData("elit.")]
        [InlineData("Sed ac")]
        [InlineData("  egestas ")]
        [InlineData("tellus. 12")]
        [InlineData("34 Etiam hendrerit")]
        [InlineData("lectus id nisl consequat semper")]
        [InlineData("Nunc ")]
        [InlineData("t.r.i.s.t.i.q.u.e")]
        [InlineData("nunc\nid libero")]
        [InlineData("\nfinibus efficitur\n5")]
        [InlineData("Etiam consequat quam sed venenatis laoreet")]
        public void EncryptDecryptCryptoanalysisReturnsCorrectKey2(string input)
        {
            var key = new Key(17, 3);
            var encrypted = _cipher.Encrypt(input, key);
            var decrypted = _cipher.Decrypt(encrypted, key);
            var foundKey = _cipher.RunCryptoanalysisWithPlain(decrypted, encrypted);

            Assert.Equal(input, decrypted);
            Assert.Equal(key.Multiplier, foundKey.Multiplier);
            Assert.Equal(key.Addend, foundKey.Addend);
        }

        [Fact]
        public void GetPossibleKeysReturns312()
        {
            Assert.Equal(312, _cipher.GetPossibleKeys().Count());
        }
    }
}
