using System;
using System.Numerics;

namespace Yandex5._4;

public static class C4E
{
    public static void Solution()
    {
        var n = BigInteger.Parse(Console.ReadLine()!);

        var prevLevel = rBinSearch(0, n, m =>
        {
            var sum = CalcSumUpTo(m);
            return sum < n;
        });

        var curLevel = prevLevel + 1;
        var curDelta = n - CalcSumUpTo(prevLevel);
        curDelta--;
        if ((curLevel) % 2 == 1)
        {
            curDelta = curLevel - curDelta - 1;
        }

        var curI = curLevel;
        BigInteger curJ = 1;
        curI -= curDelta;
        curJ += curDelta;

        Console.WriteLine($"{curI}/{curJ}");
    }

    private static BigInteger CalcSumUpTo(BigInteger m)
    {
        return (0 + m) * (m + 1) / 2;
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
        var m = 4;
        var result = (0 + m) * (m + 1) / 2;
    }
}