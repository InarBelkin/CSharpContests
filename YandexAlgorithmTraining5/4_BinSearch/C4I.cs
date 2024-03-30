using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex5._4;

public static class C4I
{
    public static void Solution()
    {
        var dnArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var d = (double)dnArr[0];
        var n = dnArr[1];
        var players = Enumerable.Range(0, n).Select(_ =>
        {
            var xyvArr = Console.ReadLine()!.Trim().Split().Select(double.Parse).ToList();
            var x = xyvArr[0];
            var y = xyvArr[1];
            var v = xyvArr[2];

            return new Player(new Vector(x, y), v);
        }).ToList();
        InnerSolution(d, players);
    }

    private static void InnerSolution(double D, List<Player> players)
    {
        var resultTime = binSearch(0, 10000000, m => Checker(D, players, m));
        Console.WriteLine(resultTime);
        var coordinates = GetIntersectionsInsideOfDArea(D, players, resultTime);
        var freeCoordinates = GetAllFreePoints(coordinates, players, resultTime);
        Console.WriteLine($"{freeCoordinates[0].X} {freeCoordinates[0].Y}");
    }

    private static bool Checker(double D, List<Player> players, double time)
    {
        //Here are only points that are inside of our D-area
        var points = GetIntersectionsInsideOfDArea(D, players, time);

        if (points.Count == 0)
        {
            return !CheckThatDIsFullInsideOfAnotherCircles(D, players, time);
        }

        var freePoints = GetAllFreePoints(points, players, time);
        return freePoints.Count > 0;
    }

    private static IReadOnlyList<Vector> GetIntersectionsInsideOfDArea(double D, List<Player> players, double time)
    {
        var points = players
            .SelectMany(p => players.Select(p2 => (p1: p, p2: p2)))
            .SelectMany(pair => GetIntersections(pair.p1.Coordinates, pair.p1.speed * time, pair.p2.Coordinates,
                pair.p2.speed * time))
            .Where(v => v.Y >= 0 && Vector.Distance(new(0, 0), v) <= D)
            .ToList();

        points.AddRange(players
            .SelectMany(p => GetIntersections(p.Coordinates, p.speed * time, new Vector(0, 0), D))
            .Where(v => v.Y >= 0));

        points.AddRange(players.SelectMany(p => GetIntersectionsWithDown(D, p, time)));

        return points;
    }


    private static bool CheckThatDIsFullInsideOfAnotherCircles(double D, List<Player> players, double time)
    {
        var pointToCheck = new Vector(D, 0);
        return players.Any(p => Vector.Distance(p.Coordinates, pointToCheck) < p.speed * time);
    }

    private static IReadOnlyList<Vector> GetAllFreePoints(IReadOnlyList<Vector> points, List<Player> players,
        double time)
    {
        var freePoints = points
            .Where(p => players.All(player => Vector.Distance(p, player.Coordinates) > player.speed * time - Precision))
            .ToList();
        return freePoints;
    }

    private static IReadOnlyList<Vector> GetIntersectionsWithDown(double D, Player player, double time)
    {
        var underSq = Math.Pow(player.speed * time, 2) - Math.Pow(player.Coordinates.Y, 2);
        if (underSq < 0)
        {
            return Array.Empty<Vector>();
        }

        var x1 = Math.Sqrt(underSq) + player.Coordinates.X;
        var x2 = -Math.Sqrt(underSq) + player.Coordinates.X;
        return new[] { new Vector(x1, 0), new Vector(x2, 0) }.Where(v => v.X <= D && v.X >= -D).ToList();
    }

    private record Player(Vector Coordinates, double speed);

    private record Vector(double X, double Y)
    {
        public static double Distance(Vector v1, Vector v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }
    }


    private const double Precision = 0.0001;

    private static double binSearch(double l, double r, Func<double, bool> check)
    {
        while (r - l >= Precision)
        {
            var m = (l + r) / 2;
            if (check(m))
            {
                l = m;
            }
            else
            {
                r = m;
            }
        }

        return l;
    }

    private static IReadOnlyList<Vector> GetIntersections(Vector center1, double rad1, Vector center2, double rad2)
    {
        var x1 = center1.X;
        var y1 = center1.Y;
        var x2 = center2.X;
        var y2 = center2.Y;
        var d = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        if (d > rad1 + rad2 || (d < Math.Max(rad1, rad2) && Math.Max(rad1, rad2) - d < Math.Min(rad1, rad2)))
            return Array.Empty<Vector>();

        var l = (Math.Pow(rad1, 2) - Math.Pow(rad2, 2) + Math.Pow(d, 2)) / (2 * d);
        var a = Math.Pow(rad1, 2) - Math.Pow(l, 2);
        var h = Math.Sqrt(a);

        var xTop = (l / d) * (x2 - x1) + (h / d) * (y2 - y1) + x1;
        var xDown = (l / d) * (x2 - x1) - (h / d) * (y2 - y1) + x1;

        var yTop = (l / d) * (y2 - y1) - (h / d) * (x2 - x1) + y1;
        var yDown = (l / d) * (y2 - y1) + (h / d) * (x2 - x1) + y1;

        var result = xTop.PresEquals(xDown) && yTop.PresEquals(yDown)
            ? new List<Vector> { new Vector(xTop, yTop) }
            : new List<Vector> { new Vector(xTop, yTop), new Vector(xDown, yDown) };
        //  result = result.Where(v => v.Y is not Double.NaN && v.X is not Double.NaN).ToList();
        return result;
    }

    private static bool PresEquals(this double number, double another)
    {
        return Math.Abs(number - another) < Precision;
    }

    public static void SomeTests()
    {
        var intersections = GetIntersections(new Vector(4, 4), 4, new Vector(5, 5), 1);
        Console.WriteLine(intersections.Count);
    }
}