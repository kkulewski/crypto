using System;
using AffineCipher;
using Xunit;

namespace Test
{
    public class CaesarCipherTests
    {
        private readonly Cipher _cipher = new CaesarCipher();

        [Fact]
        public void EncryptWithKey7ConvertsTestToAlza()
        {
            var input = "Test";
            var output = _cipher.Encrypt(input, 7);

            Assert.Equal("Alza", output);
        }

        [Fact]
        public void DecryptWithKey7ConvertsAlzaToTest()
        {
            var input = "Alza";
            var output = _cipher.Decrypt(input, 7);

            Assert.Equal("Test", output);
        }

        [Fact]
        public void EncryptAndDecryptCaesarReturnsOriginalInput()
        {
            var key = 13;
            var input =
                "Look again at that dot. That\'s here. That\'s home. That\'s us. " +
                "On it everyone you love, everyone you know, everyone you ever heard of, " +
                "every human being who ever was, lived out their lives. The aggregate of our joy and suffering, " +
                "thousands of confident religions, ideologies, and economic doctrines, every hunter and forager, " +
                "every hero and coward, every creator and destroyer of civilization, every king and peasant, " +
                "every young couple in love, every mother and father, hopeful child, inventor and explorer, " +
                "every teacher of morals, every corrupt politician, every \"superstar,\" every \"supreme leader,\" " +
                "every saint and sinner in the history of our species lived there-on a mote of dust suspended in a sunbeam.";

            var encrypted = _cipher.Encrypt(input, key);
            var decrypted = _cipher.Decrypt(encrypted, key);

            Assert.Equal(decrypted, input);
        }

        [Fact]
        public void RunCryptoanalysisWithPlainReturnsCorrectKey()
        {
            var key = 11;
            var input = "Test";
            var output = _cipher.Encrypt(input, key);

            Assert.Equal(_cipher.RunCryptoanalysisWithPlain(input, output), key);
        }

        [Fact]
        public void RunCryptoanalysisWithPlainMixedWithNumbersAndPunctuationReturnsCorrectKey()
        {
            var key = 22;
            var input = ".3Tes1t \n14 A, CC5.";
            var output = _cipher.Encrypt(input, key);

            Assert.Equal(_cipher.RunCryptoanalysisWithPlain(input, output), key);
        }
    }
}
