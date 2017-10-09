using System;
using AffineCipher;
using Xunit;

namespace Test
{
    public class CipherTests
    {
        [Fact]
        public void CaesarWithKey7ConvertsTestToAlza()
        {
            var input = "Test";
            var output = Cipher.CipherCaesar(input, 7);

            Assert.Equal("Alza", output);
        }
    }
}
