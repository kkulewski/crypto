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
            var input = "Test";
            var output = Cipher.EncryptCaesar(input, 7);

            Assert.Equal("Alza", output);
        }
    }
}
