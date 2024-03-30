using System;
using System.Linq;

namespace Yandex5._4;

public static class C4D
{
    public static void Solution()
    {
        var wnmArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var w = wnmArr[0];
        var firstRap = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToArray();
        var secondRap = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToArray();

        var firstMin = firstRap.MaxBy(r => r);
        var secondMin = secondRap.MaxBy(r => r);

        var result = lBinSearch(firstMin, w - secondMin, m =>
        {
            var leftHeight = CalculateHeight(firstRap, m);
            var rightHeight = CalculateHeight(secondRap, w - m);

            return rightHeight >= leftHeight;
        });

        if (result == firstMin)
        {
            var leftHeightRes = CalculateHeight(firstRap, result);
            var rightHeightRes = CalculateHeight(secondRap, w - result);

            Console.WriteLine(Math.Max(leftHeightRes, rightHeightRes));
        }
        else
        {
            var rpointHeight = Math.Max(CalculateHeight(firstRap, result), CalculateHeight(secondRap, w - result));
            var lpointHeight = Math.Max(CalculateHeight(firstRap, result - 1),
                CalculateHeight(secondRap, w - result + 1));
            Console.WriteLine(Math.Min(lpointHeight, rpointHeight));
        }
    }

    private static int CalculateHeight(int[] raport, int width)
    {
        var currentRow = 0;
        var currentColumn = 0;
        foreach (var wordLen in raport)
        {
            if (currentColumn + wordLen + (currentColumn == 0 ? 0 : 1) > width)
            {
                currentRow++;
                currentColumn = wordLen;
            }
            else
            {
                currentColumn += wordLen + (currentColumn == 0 ? 0 : 1);
            }
        }

        return currentRow + 1;
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

    public static void SomeTest1()
    {
        var rap = "2 2 4 5 7\n".Trim().Split().Select(int.Parse).ToArray();
        var result = CalculateHeight(rap, 7);
    }
}