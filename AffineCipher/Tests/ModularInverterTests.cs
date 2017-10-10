using System;
using AffineCipher;
using Xunit;

namespace Test
{
    public class ModularInverterTests
    {
        [Fact]
        public void InversionOf7Mod26Exists()
        {
            Assert.True(Modulo.GetInversionNaive(7, 26, out int _));
        }

        [Fact]
        public void InversionOf7Mod26Returns15()
        {
            Modulo.GetInversionNaive(7, 26, out int inverse);
            Assert.Equal(15, inverse);
        }

        [Fact]
        public void InversionOf4Mod26DoesNotExist()
        {
            Assert.False(Modulo.GetInversionNaive(4, 26, out int _));
        }

        [Fact]
        public void ReturnsZeroWhenInverseDoesNotExist()
        {
            Modulo.GetInversionNaive(4, 26, out int inverse);
            Assert.Equal(0, inverse);
        }
    }
}
