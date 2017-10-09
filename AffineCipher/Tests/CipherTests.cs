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

            Assert.Equal(input, input);
        }
    }
}
