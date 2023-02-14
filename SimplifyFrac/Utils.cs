using System;

namespace SimplifyFrac
{
    class Utils
    {
        public static Int64 Power(int value, int pow)
        {
            var result = value;
            while (pow-- > 1)
                result *= value;
            return pow == 0 ? result : pow == -1 ? 1 : throw new ArgumentOutOfRangeException(nameof(pow));
        }

        public static Int64 GetIntegerFromPrimeFactors(int[] x)
        {
            var primes = new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
            Int64 product = 1;

            for (int i1 = 0; i1 < x.Length; i1++)
            {
                product *= Power(primes[i1], x[i1]);
            }

            if (product < 0)
            {
                global::System.Console.WriteLine();
            }
            return product;
        }
    }
}
