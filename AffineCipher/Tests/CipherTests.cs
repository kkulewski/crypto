using System;
using AffineCipher;
using Xunit;

namespace Test
{
    public class CipherTests
    {
        [Fact]
        public void EncryptCaesarWithKey7ConvertsTestToAlza()
        {
            var cipher = new Cipher();

            var input = "Test";
            var output = cipher.EncryptCaesar(input, 7);

            Assert.Equal("Alza", output);
        }

        [Fact]
        public void DecryptCaesarWithKey7ConvertsAlzaToTest()
        {
            var cipher = new Cipher();

            var input = "Alza";
            var output = cipher.DecryptCaesar(input, 7);

            Assert.Equal("Test", output);
        }

        [Fact]
        public void EncryptCaesarAndDecryptCaesarReturnsOriginalInput()
        {
            var cipher = new Cipher();

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

            var encrypted = cipher.EncryptCaesar(input, key);
            var decrypted = cipher.DecryptCaesar(encrypted, key);

            Assert.Equal(decrypted, input);
        }

        [Fact]
        public void RunCaesarCryptoanalysisWithPlainReturnsCorrectKey()
        {
            var cipher = new Cipher();

            var key = 11;
            var input = "Test";
            var output = cipher.EncryptCaesar(input, key);

            Assert.Equal(cipher.RunCaesarCryptoanalysisWithPlain(input, output), key);
        }

        [Fact]
        public void RunCaesarCryptoanalysisWithPlainMixedWithNumbersAndPunctuationReturnsCorrectKey()
        {
            var cipher = new Cipher();

            var key = 22;
            var input = ".3Tes1t \n14 A, CC5.";
            var output = cipher.EncryptCaesar(input, key);

            Assert.Equal(cipher.RunCaesarCryptoanalysisWithPlain(input, output), key);
        }
    }
}
