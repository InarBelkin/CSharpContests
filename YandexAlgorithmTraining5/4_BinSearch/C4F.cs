using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex5._4;

public static class C4F
{
    public static void Solution()
    {
        var whnArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var W = whnArr[0];
        var H = whnArr[1];
        var N = whnArr[2];

        var points = Enumerable.Range(0, N).Select(_ =>
        {
            var xyarr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
            return new Vec(xyarr[0] - 1, xyarr[1] - 1);
        }).ToList();

        var verticals =
            points
                .GroupBy(p => p.X)
                .Select(g => new PointVertical(g.Key, g.ToList()))
                .OrderBy(p => p.X)
                .ToArray();

        var fromLeftVerticalsMins = CalculateFromLeftVerticalMins(verticals);
        var fromRightVerticalsMins = CalculateFromRightVerticalMins(verticals);

        var result = lBinSearch(1, Math.Min(W, H),
            m => Checker(m, verticals, fromLeftVerticalsMins, fromRightVerticalsMins, W, H));
        Console.WriteLine(result);
    }

    private static bool Checker(int m, PointVertical[] verticals, VerticalWithMinWidth[] fromLeft,
        VerticalWithMinWidth[] fromRight, int fieldWidth, int fieldHeight)
    {
        int? minHorWidth = null;
        var rightP = 0;
        for (int leftP = 0; leftP < verticals.Length; leftP++)
        {
            while (rightP + 1 < verticals.Length && verticals[rightP + 1].X < verticals[leftP].X + m)
            {
                rightP++;
            }

            if (leftP > 0 && rightP < verticals.Length - 1)
            {
                var bothMax1 = Math.Max(fromLeft[leftP - 1].MaxY, fromRight[rightP + 1].MaxY);
                var bothMin1 = Math.Min(fromLeft[leftP - 1].MinY, fromRight[rightP + 1].MinY);
                var curHorWidth1 = bothMax1 - bothMin1 + 1;
                if (minHorWidth is null || curHorWidth1 < minHorWidth)
                {
                    minHorWidth = curHorWidth1;
                }
            }
            else if (leftP > 0)
            {
                var curHorWidth1 = fromLeft[leftP - 1].MaxY - fromLeft[leftP - 1].MinY + 1;
                if (minHorWidth is null || curHorWidth1 < minHorWidth)
                {
                    minHorWidth = curHorWidth1;
                }
            }
            else if (rightP < verticals.Length - 1)
            {
                var curHorWidth1 = fromRight[rightP + 1].MaxY - fromRight[rightP + 1].MinY + 1;
                if (minHorWidth is null || curHorWidth1 < minHorWidth)
                {
                    minHorWidth = curHorWidth1;
                }
            }
            else
            {
                minHorWidth = 0;
            }
        }

        return minHorWidth!.Value <= m;
    }


    private static VerticalWithMinWidth[] CalculateFromLeftVerticalMins(PointVertical[] verticals)
    {
        var leftMaxY = verticals[0].Vecs.Select(v => v.Y).MaxBy(y => y);
        var leftMinY = verticals[0].Vecs.Select(v => v.Y).Min(y => y);
        var fromLeftVerticalsMins = new List<VerticalWithMinWidth>(verticals.Length)
            { new VerticalWithMinWidth(verticals[0].X, leftMaxY, leftMinY) };

        foreach (var (x, vecs) in verticals.Skip(1))
        {
            var curMaxY = vecs.Select(v => v.Y).MaxBy(y => y);
            var curMinY = vecs.Select(v => v.Y).Min(y => y);
            leftMaxY = Math.Max(leftMaxY, curMaxY);
            leftMinY = Math.Min(leftMinY, curMinY);
            fromLeftVerticalsMins.Add(new VerticalWithMinWidth(x, leftMaxY, leftMinY));
        }

        return fromLeftVerticalsMins.ToArray();
    }

    private static VerticalWithMinWidth[] CalculateFromRightVerticalMins(PointVertical[] verticals)
    {
        var rightMaxY = verticals[^1].Vecs.Select(v => v.Y).MaxBy(y => y);
        var rightMinY = verticals[^1].Vecs.Select(v => v.Y).MinBy(y => y);

        var fromRightVerticalsMins = new VerticalWithMinWidth[verticals.Length];
        fromRightVerticalsMins[verticals.Length - 1] =
            new VerticalWithMinWidth(verticals[^1].X, rightMaxY, rightMinY);

        for (int i = verticals.Length - 2; i >= 0; i--)
        {
            var curMaxY = verticals[i].Vecs.Select(v => v.Y).MaxBy(y => y);
            var curMinY = verticals[i].Vecs.Select(v => v.Y).MinBy(y => y);
            rightMaxY = Math.Max(rightMaxY, curMaxY);
            rightMinY = Math.Min(rightMinY, curMinY);
            fromRightVerticalsMins[i] = new VerticalWithMinWidth(verticals[i].X, rightMaxY, rightMinY);
        }

        return fromRightVerticalsMins;
    }

    record struct Vec(int X, int Y);

    record struct PointVertical(int X, List<Vec> Vecs);

    record struct VerticalWithMinWidth(int X, int MaxY, int MinY);

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
}