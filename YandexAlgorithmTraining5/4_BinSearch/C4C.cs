using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Yandex5._4;

public static class C4C
{
    public static void Solution()
    {
        using var file = new StreamReader(@"F:\Projects\Textes\input.txt");
        var nmArr = file.ReadLine()!.Trim().Split().Select(long.Parse).ToList();
        var n = nmArr[0];
        var cntOfQuestions = nmArr[1];
        var polks = file.ReadLine()!.Trim().Split().Select(long.Parse).ToArray();
        var prefSums = calcPrefixSum(polks);
        var sumResult = new StringBuilder();
        for (int i = 0; i < cntOfQuestions; i++)
        {
            var lsArr = file.ReadLine()!.Trim().Split().Select(long.Parse).ToList();
            var cntOfPolks = lsArr[0];
            var targCnt = lsArr[1];

            var result = lBinSearch(0, polks.Length - cntOfPolks, m =>
            {
                var curSum = prefSums[m + cntOfPolks] - prefSums[m];
                return curSum >= targCnt;
            });
            if (prefSums[result + cntOfPolks] - prefSums[result] == targCnt)
            {
                sumResult.AppendLine((result + 1).ToString());
            }
            else
            {
                sumResult.AppendLine("-1");
            }
        }

        Console.WriteLine(sumResult);
    }

    static long[] calcPrefixSum(long[] arr)
    {
        var prefSums = new long[arr.Length + 1];
        for (long i = 1; i <= arr.Length; i++)
        {
            prefSums[i] = arr[i - 1] + prefSums[i - 1];
        }

        return prefSums;
    }

    private static long lBinSearch(long l, long r, Func<long, bool> check)
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
}