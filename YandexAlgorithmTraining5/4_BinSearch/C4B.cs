using System;
using System.Linq;
using System.Numerics;

namespace Yandex5._4;

public static class C4B
{
    public static void Solution()
    {
        var highBound = BigInteger.Parse("1000000");
        var n = BigInteger.Parse(Console.ReadLine()!);
        if (n == 0)
        {
            Console.WriteLine(0);
            return;
        }

        var result = rBinSearch(1, highBound, k =>
        {
            if (k == 1) return true;
            BigInteger sumLenght = 0;
            for (BigInteger r = 0; r < k; r++)
            {
                sumLenght += (2 + r) * (k - r);
            }

            sumLenght--;

            return sumLenght <= n;
        });
        Console.WriteLine(result);
    }

    private static int lBinSearch(int l, int r, Func<int, bool> check)
    {
        while (l < r)
        {
            var m = (l + r) / 2;
            if (check(m))
            {
                r = m;
            }
            else
            {
                l = m + 1;
            }
        }

        return l;
    }

    public static BigInteger rBinSearch(BigInteger l, BigInteger r, Func<BigInteger, bool> check)
    {
        while (l < r)
        {
            var m = (l + r + 1) / 2;
            if (check(m))
            {
                l = m;
            }
            else
            {
                r = m - 1;
            }
        }

        return l;
    }

    public static void SomeTest()
    {
        var list = Enumerable.Range(2, 3).Select(i => Calc2(i)).ToList();
    }

    private static int Calc(int n)
    {
        return (n * n * n + 6 * n * n - 3 * n - 4) / 6;
    }

    private static int Calc2(int n)
    {
        return n * n + n * n * n / 6;
    }
}