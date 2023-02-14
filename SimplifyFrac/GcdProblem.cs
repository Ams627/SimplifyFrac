using System;
using System.Collections.Generic;
using System.Linq;

namespace SimplifyFrac
{
    class GcdProblem
    {
        public GcdProblem(List<ComposedInteger> integers)
        {
            Integers = integers;
        }

        public GcdProblem(params ComposedInteger[] integers)
        {
            foreach (var entry in integers)
            {
                Integers.Add(entry);
            }
        }


        public List<ComposedInteger> Integers { get; } = new List<ComposedInteger>();
        public Int64 GetGcd()
        {
            var intLists = Integers.Select(x => x.PrimeFactorPowers).ToArray();
            var l = intLists[0].Length;
            if (!intLists.All(x => x.Length == l))
            {
                throw new ArgumentException("Int lists must all be the same length");
            }

            var tranposed = Transpose(intLists);
            var primesArray = tranposed.Select(x => x.Min()).ToArray();
            var product = Utils.GetIntegerFromPrimeFactors(primesArray);
            return product;
        }


        public int[] GetCommonPrimeFactors()
        {
            var intLists = Integers.Select(x => x.PrimeFactorPowers).ToArray();
            var l = intLists[0].Length;
            if (!intLists.All(x => x.Length == l))
            {
                throw new ArgumentException("Int lists must all be the same length");
            }

            var tranposed = Transpose(intLists);
            var primesArray = tranposed.Select(x => x.Min()).ToArray();
            return primesArray;
        }


        public static T[][] Transpose<T>(T[][] arr)
        {
            int rowCount = arr.Length;
            int columnCount = arr[0].Length;
            T[][] transposed = new T[columnCount][];
            if (rowCount == columnCount)
            {
                transposed = (T[][])arr.Clone();
                for (int i = 1; i < rowCount; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        T temp = transposed[i][j];
                        transposed[i][j] = transposed[j][i];
                        transposed[j][i] = temp;
                    }
                }
            }
            else
            {
                for (int column = 0; column < columnCount; column++)
                {
                    transposed[column] = new T[rowCount];
                    for (int row = 0; row < rowCount; row++)
                    {
                        transposed[column][row] = arr[row][column];
                    }
                }
            }
            return transposed;
        }
    }
}
