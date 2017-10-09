using System;
using AffineCipher;
using Xunit;

namespace Tests
{
    public class ModularInverterTest
    {
        [Fact]
        public void InversionOf7Mod26Exists()
        {
            Assert.True(ModularInverter.GetInversionNaive(7, 26, out int _));
        }

        [Fact]
        public void InversionOf7Mod26Returns15()
        {
            ModularInverter.GetInversionNaive(7, 26, out int inverse);
            Assert.Equal(15, inverse);
        }

        [Fact]
        public void InversionOf4Mod26DoesNotExist()
        {
            Assert.False(ModularInverter.GetInversionNaive(4, 26, out int _));
        }

        [Fact]
        public void ReturnsZeroWhenInverseDoesNotExist()
        {
            ModularInverter.GetInversionNaive(4, 26, out int inverse);
            Assert.Equal(0, inverse);
        }
    }
}
