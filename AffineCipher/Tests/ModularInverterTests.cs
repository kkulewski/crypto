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
            Assert.True(ModuloHelpers.GetInversionNaive(7, 26, out int _));
        }

        [Fact]
        public void InversionOf7Mod26Returns15()
        {
            ModuloHelpers.GetInversionNaive(7, 26, out int inverse);
            Assert.Equal(15, inverse);
        }

        [Fact]
        public void InversionOf4Mod26DoesNotExist()
        {
            Assert.False(ModuloHelpers.GetInversionNaive(4, 26, out int _));
        }

        [Fact]
        public void ReturnsZeroWhenInverseDoesNotExist()
        {
            ModuloHelpers.GetInversionNaive(4, 26, out int inverse);
            Assert.Equal(0, inverse);
        }
    }
}
