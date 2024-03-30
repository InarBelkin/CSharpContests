using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Yandex5._4;

public static class C4G
{
    public static void Solution()
    {
        using var file = new StreamReader(@"F:\Projects\Textes\input.txt");
        var nmArr = file.ReadLine()!.Split().Select(int.Parse).ToList();
        var height = nmArr[0];
        var width = nmArr[1];
        var field = Enumerable.Range(0, height).Select(_ => file.ReadLine()!).ToArray();
        var prefixes = field.Select(s => CalcPrefix(s)).ToArray();
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
            for (int r = 0; r < description.size; r++)
            {
                var prefRow = prefixes[i + r];
                var descriptionRow = description.rows[r];
                var cnt = prefRow[j + descriptionRow.finish] - prefRow[j + descriptionRow.start];

                if (cnt != descriptionRow.finish - descriptionRow.start)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public static int[] CalcPrefix(string s)
    {
        var prefix = new int[s.Length + 1];
        for (int i = 1; i <= s.Length; i++)
        {
            prefix[i] = prefix[i - 1] + (s[i - 1] == '#' ? 1 : 0);
        }

        return prefix;
    }

    private record struct PlusDescription(int size, List<(int start, int finish)> rows);

    private static PlusDescription CalcPlusDescription(int k)
    {
        List<(int start, int finish)> rows = new();
        for (int i = 0; i < k; i++)
        {
            rows.Add((k, k + k));
        }

        for (int i = 0; i < k; i++)
        {
            rows.Add((0, k * 3));
        }

        for (int i = 0; i < k; i++)
        {
            rows.Add((k, k + k));
        }

        return new PlusDescription(k * 3, rows);
    }

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