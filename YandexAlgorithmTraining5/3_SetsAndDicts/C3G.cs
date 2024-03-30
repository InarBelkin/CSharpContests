using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex5;

public static class C3G
{
    public static void G()
    {
        var N = int.Parse(Console.ReadLine()!);
        var points = Enumerable.Range(0, N).Select(_ =>
        {
            var arr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
            return new Vec(arr[0], arr[1]);
        }).ToList();
        var pointsHash = new HashSet<Vec>(points);

        List<Vec>? bestLeft = null;
        for (int i = 0; i < N - 1; i++)
        {
            for (int j = i + 1; j < N; j++)
            {
                var listOfListsToCheck = GetLeftVectors(points[i], points[j]);
                foreach (var pointsToCheck in listOfListsToCheck)
                {
                    var leftPoints = pointsToCheck.Where(p => !pointsHash.Contains(p)).ToList();
                    if (bestLeft is null || bestLeft.Count > leftPoints.Count)
                    {
                        bestLeft = leftPoints;
                    }
                }
            }
        }

        if (bestLeft is null)
        {
            var point = points[0];
            bestLeft = new List<Vec>()
            {
                new Vec(point.x + 1, point.y), new Vec(point.x + 1, point.y + 1), new Vec(point.x, point.y + 1)
            };
        }

        Console.WriteLine(bestLeft.Count);

        foreach (var (x, y) in bestLeft)
        {
            Console.WriteLine($"{x} {y}");
        }
    }

    static IReadOnlyList<IReadOnlyList<Vec>> GetLeftVectors(Vec vec1, Vec vec2)
    {
        var vertDist = Math.Abs(vec2.y - vec1.y);
        var horizontDist = Math.Abs(vec2.x - vec1.x);
        if (vec1.x == vec2.x)
        {
            return new[]
            {
                new[] { new Vec(vec1.x + vertDist, vec1.y), new Vec(vec2.x + vertDist, vec2.y) },
                new[] { new Vec(vec1.x - vertDist, vec1.y), new Vec(vec2.x - vertDist, vec2.y) }
            };
        }
        else if (vec1.y == vec2.y)
        {
            return new[]
            {
                new[] { new Vec(vec1.x, vec1.y + horizontDist), new Vec(vec2.x, vec2.y + horizontDist) },
                new[] { new Vec(vec1.x, vec1.y - horizontDist), new Vec(vec2.x, vec2.y - horizontDist) }
            };
        }
        else if (vertDist == horizontDist)
        {
            return new[]
            {
                new[] { new Vec(vec2.x, vec1.y), new Vec(vec1.x, vec2.y) }
            };
        }

        return Array.Empty<IReadOnlyList<Vec>>();
    }

    record class Vec(int x, int y)
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

    public static void G2()
    {
        var N = int.Parse(Console.ReadLine()!);
        var points = Enumerable.Range(0, N).Select(_ =>
        {
            var arr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
            return new Vec(arr[0], arr[1]);
        }).ToArray();
        var pointsHash = new HashSet<Vec>(points);

        List<Vec>? bestLeft = null;
        for (int i = 0; i < N - 1; i++)
        {
            for (int j = i + 1; j < N; j++)
            {
                var listOfListsToCheck = GetLeftVectors2(points[i], points[j]);
                foreach (var pointsToCheck in listOfListsToCheck)
                {
                    var leftPoints = pointsToCheck.Where(p => !pointsHash.Contains(p)).ToList();
                    if (bestLeft is null || bestLeft.Count > leftPoints.Count)
                    {
                        bestLeft = leftPoints;
                    }
                }
            }
        }

        if (bestLeft is null)
        {
            var point = points[0];
            bestLeft = new List<Vec>()
            {
                new Vec(point.x + 1, point.y), new Vec(point.x + 1, point.y + 1), new Vec(point.x, point.y + 1)
            };
        }

        Console.WriteLine(bestLeft.Count);

        foreach (var (x, y) in bestLeft)
        {
            Console.WriteLine($"{x} {y}");
        }
    }

    const double cos = 0.70710678118;
    const double precision = 1 / 1000_000_00;

    static IReadOnlyList<IReadOnlyList<Vec>> GetLeftVectors2(Vec vec1, Vec vec2)
    {
        var sideVec = vec2 - vec1;

        var rotatedSideVec = new Vec(-sideVec.y, sideVec.x);
        var list1 = new[] { vec1 + rotatedSideVec, vec2 + rotatedSideVec };
        var list2 = new[] { vec1 - rotatedSideVec, vec2 - rotatedSideVec };
        var positiveRotated = (cos * sideVec.x - cos * sideVec.y, cos * sideVec.y + cos * sideVec.y);
        var nSin = -cos;
        var negativeRotated = (cos * sideVec.x - nSin * sideVec.y, nSin * sideVec.x);
        List<Vec>? list3 = null;
        if (Math.Abs(positiveRotated.Item1 % 1.0) < precision && Math.Abs(positiveRotated.Item2 % 1.0) < precision)
        {
            list3 = new List<Vec>()
            {
                new Vec((int)Math.Abs(positiveRotated.Item1), (int)Math.Abs(positiveRotated.Item2)),
                new Vec((int)Math.Abs(negativeRotated.Item1), (int)Math.Abs(negativeRotated.Item2))
            };
        }

        var result = new List<IReadOnlyList<Vec>>() { list1, list2 };
        if (list3 is not null)
        {
            result.Add(list3);
        }

        return result;
    }
}