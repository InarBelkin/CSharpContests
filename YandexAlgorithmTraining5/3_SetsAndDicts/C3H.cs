using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex5._3;

public static class C3H
{
    record struct Vec(int x, int y)
    {
        public static Vec operator +(Vec a, Vec b)
        {
            return new Vec(a.x + b.x, a.y + b.y);
        }

        public static Vec operator -(Vec a, Vec b)
        {
            return new Vec(a.x - b.x, a.y - b.y);
        }
    }

    public static void Solution()
    {
        var n = int.Parse(Console.ReadLine()!);
        var fromArr = Enumerable.Range(0, n).Select(_ => ReadCoordinates()).ToList();
        var toArr = Enumerable.Range(0, n).Select(_ => ReadCoordinates()).ToList();

        var fromSet = new HashSet<(Vec, Vec)>(fromArr);

        var diffs = new Dictionary<Vec, int>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (GetZeroVector(fromArr[i]) == GetZeroVector(toArr[j]))
                {
                    var lineFrom = fromArr[i];
                    var lineTo = toArr[j];
                    var diff = lineTo.Item1 - lineFrom.Item1;
                    diffs.TryAdd(diff, 0);
                    diffs[diff]++;
                }
            }
        }

        Console.WriteLine(diffs.Count == 0 ? n : n - diffs.Values.Max());
    }


    private static (Vec, Vec) ReadCoordinates()
    {
        var arr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var a = new Vec(arr[0], arr[1]);
        var b = new Vec(arr[2], arr[3]);

        if (b.y < a.y || (a.y == b.y && a.x > b.x))
        {
            (a, b) = (b, a);
        }

        return (a, b);
    }

    private static Vec GetZeroVector((Vec a, Vec b) line)
    {
        return GetZeroVector(line.a, line.b);
    }

    private static Vec GetZeroVector(Vec a, Vec b)
    {
        return a - b;
    }

    public static void Test1()
    {
        var line1 = ReadCoordinates();
        var line2 = ReadCoordinates();
        var z1 = GetZeroVector(line1);
        var z2 = GetZeroVector(line2);
    }
}