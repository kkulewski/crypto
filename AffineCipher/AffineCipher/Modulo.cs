namespace AffineCipher
{
    public static class Modulo
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

        public static bool RelativelyPrime(int a, int b)
        {
            return GreatestCommonDivisor(a, b) == 1;
        }

        public static int GreatestCommonDivisor(int a, int b)
        {
            while (b > 0)
            {
                var x = a % b;
                a = b;
                b = x;
            }
            return a;
        }
    }
}
