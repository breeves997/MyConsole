using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsole.Commands
{
    public static class MathGames
    {
        public static string TwistedSum(long n)
        {
            var numArr = n.ToString().Select(c => (int)Char.GetNumericValue(c)).ToList();
            int length = numArr.Count();
            int x;
            double factor;
            double total = 0;
            for (int i = 0; i < length; i++)
            {
                x = numArr[i];
                factor = x * Math.Pow(10, length - 2 - i);
                if (i == 0)
                    total += (GetSumToN(x - 1) * Math.Pow(10, length - 1) + 45 * factor * (length - 1) + x);
                else //get remainder portion to the left of the current digit
                {
                    total += (i < length - 1) ? 45 * factor : GetSumToN(numArr[i]);
                    for (int j = 0; j < i; j++) //add remaining digits not counted in loop
                    {
                        if (i == length - 1)
                        {
                            total += (j != i-1) ? (x + 1) * numArr[j] : 0;
                            if (j == i - 1) total += GetSumToN(numArr[length - 2] - 1) * 10;
                        }
                        else
                            total += x * Math.Pow(10, length - 1 - i) * numArr[j];
                    }
                }

            }
            //for (int i = 0; i < length; i++)
            //{
            //    x = numArr[i];
            //    factor = x * Math.Pow(10, length - 2 - i);
            //    if (i == 0)
            //        total += GetSumToN(x) * Math.Pow(10, length - 1);
            //    else if (i == length - 1)
            //        total += GetSumToN(x);
            //    else //get remainder portion to the left of the current digit
            //    {
            //        total += (45 + GetSumToN(x)) * Math.Pow(10, length - 2 - i);
            //    }

            //}

            return total.ToString();
        }

        public static long GetSumToN(int n)
        {
            int count = 0;
            for (int i = 0; i <= n; i++)
            {
                count += i;
            }
            return count;
        }

        public static string ArrayDecomposition (long n)
        {
            //from codata:
            //{
            //    var l = new List<long>();
            //    for (int i = 2; i * i <= n; i++)
            //    {
            //        long factor = (long)Math.Log(n, i);
            //        l.Add(factor);
            //        n -= (long)Math.Pow(i, factor);
            //    }
            //    return new long[][] { l.ToArray(), new long[] { n } };
            //}
            long[][] ans = new long[2][];
            List<long> decomp = new List<long>();
            double x;
            double i = 2;
            double counter = n;
            long remainder = 0;
            while (counter > 0)
            {
                if (counter - (i) * (i) < 0)
                {
                    remainder = (long)counter;
                    break;
                }
                x = Math.Floor(Math.Log(counter, i));
                counter -= (x > 0) ? Math.Pow(i, x) : 0;

                decomp.Add((long)x);
                if (counter == 0)
                    remainder = 0;
                i++;
            }

            ans[0] = decomp.ToArray();
            ans[1] = new long[1] { remainder };
            return ToolShed.ArrToStr(ans[0]) + ToolShed.ArrToStr(ans[1]); ;
        }



    }

}

