using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsole.Commands
{
    public static class Factorization
    {

        public static string PrimeFactors(int n)
        {
            var factors = new List<Tuple<int, int>>();
            factors = ReturnFactors(n, GeneratePrimesSieveOfSundaram(n));
            return "z";
        }

        public static List<Tuple<int, int>> ReturnFactors(int x, List<int> primes)
        {
            var factors = new List<Tuple<int, int>>();
            foreach (int p in primes)
            {
                if (p * p > x)
                    break;
                int i = 0;
                while (x % p == 0)
                {
                    x /= p;
                    i += 1;
                }

                if (i > 0)
                {
                    var tuple = Tuple.Create(p, i);
                    factors.Add(tuple);
                }
            }

            if (x > 1)
            {
                var otherTuple = Tuple.Create(x, 1);
                factors.Add(otherTuple);
            }
            return factors;
        }

        public static BitArray SieveOfSundaram(int limit)
        {
            limit /= 2;
            BitArray bits = new BitArray(limit + 1, true);
            for (int i = 1; 3 * i + 1 < limit; i++)
            {
                for (int j = 1; i + j + 2 * i * j <= limit; j++)
                {
                    bits[i + j + 2 * i * j] = false;
                }
            }
            return bits;
        }

        public static List<int> GeneratePrimesSieveOfSundaram(int n)
        {
            int limit = ApproximateNthPrime(n);
            BitArray bits = SieveOfSundaram(limit);
            List<int> primes = new List<int>();
            primes.Add(2);
            for (int i = 1, found = 1; 2 * i + 1 <= limit && found < n; i++)
            {
                if (bits[i])
                {
                    primes.Add(2 * i + 1);
                    found++;
                }
            }
            return primes;
        }

        private static int ApproximateNthPrime(int nn)
        {
            var n = (double)nn;
            double p;
            if (nn >= 7022)
            {
                p = (n * Math.Log(n)) + (n * (Math.Log(Math.Log(n)) - 0.9385));
            }
            else if (nn >= 6)
            {
                p = (n * Math.Log(n)) + (n * Math.Log(Math.Log(n)));
            }
            else if (nn > 0)
            {
                p = new[] { 2, 3, 5, 7, 11 }[nn - 1];
            }
            else
            {
                p = 0;
            }

            return (int)p;
        }
    }

}
