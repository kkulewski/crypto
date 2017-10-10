namespace AffineCipher
{
    public static class ModuloHelpers
    {
        public static bool GetInversionNaive(int number, int modulo, out int inversion)
        {
            inversion = 0;

            for (int counter = 0; counter < modulo; counter++)
            {
                int result = (counter * number) % modulo;
                if (result == 1)
                {
                    inversion = counter;
                    return true;
                }
            }

            return false;
        }
    }
}
