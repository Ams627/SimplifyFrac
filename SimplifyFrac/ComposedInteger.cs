using System;

namespace SimplifyFrac
{
    internal class ComposedInteger
    {
        public int[] PrimeFactorPowers { get; }
        public Int64 Result { get; }

        public ComposedInteger(int[] primeFactors)
        {
            PrimeFactorPowers = primeFactors;
            Result = Utils.GetIntegerFromPrimeFactors(primeFactors);
        }
    }
}
