using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Yandex5._4;

public static class C4G2
{
    public static void Solution()
    {
        using var file = new StreamReader(@"F:\Projects\Textes\input.txt");
        var nmArr = file.ReadLine()!.Split().Select(int.Parse).ToList();
        var height = nmArr[0];
        var width = nmArr[1];
        var field = Enumerable.Range(0, height).Select(_ => file.ReadLine()!).ToArray();
        var prefixes = CalcPrefix(field);
        var result = rBinSearch(1, height / 2, m =>
        {
            var description = CalcPlusDescription(m);
            for (int i = 0; i <= height - description.size; i++)
            {
                for (int j = 0; j <= width - description.size; j++)
                {
                    var canInsert = canInsertHere(i, j, description);
                    if (canInsert)
                    {
                        return true;
                    }
                }
            }

            return false;
        });

        Console.WriteLine(result);

        bool canInsertHere(int i, int j, PlusDescription description)
        {
            foreach ((Vec rightDown, Vec leftTop) in description.squares)
            {
                var filled = IsSquareFilled(prefixes,
                    new Vec(rightDown.ii + i, rightDown.jj + j),
                    new Vec(leftTop.ii + i, leftTop.jj + j));
                if (!filled)
                    return false;
            }

            return true;
        }
    }

    private static bool IsSquareFilled(int[][] prefixes, Vec rightDown, Vec leftTop)
    {
        var sum = prefixes[rightDown.ii][rightDown.jj] - prefixes[rightDown.ii][leftTop.jj] -
            prefixes[leftTop.ii][rightDown.jj] + prefixes[leftTop.ii][leftTop.jj];

        var expectedSum = (rightDown.ii - leftTop.ii) * (rightDown.jj - leftTop.jj);
        return sum == expectedSum;
    }

    public static int[][] CalcPrefix(string[] field)
    {
        var prefix = new int[field.Length + 1][];
        for (int i = 0; i < field.Length + 1; i++)
        {
            prefix[i] = new int[field[0].Length + 1];
        }

        for (int i = 1; i <= field.Length; i++)
        {
            var rowSum = 0;
            for (int j = 1; j <= field[0].Length; j++)
            {
                rowSum += field[i - 1][j - 1] == '#' ? 1 : 0;
                prefix[i][j] = prefix[i - 1][j] + rowSum;
            }
        }

        return prefix;
    }

    private record struct PlusDescription(int size, List<(Vec rightDown, Vec leftTop)> squares);

    private static PlusDescription CalcPlusDescription(int k)
    {
        var squares = new List<(Vec rightDown, Vec leftTop)>()
        {
            (new Vec(k, 2 * k), new Vec(0, k)),
            (new Vec(2 * k, 3 * k), new Vec(k, 0)),
            (new Vec(3 * k, 2 * k), new Vec(2 * k, k))
        };
        return new PlusDescription(k * 3, squares);
    }

    private record struct Vec(int ii, int jj);

    public static int rBinSearch(int l, int r, Func<int, bool> check)
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
}