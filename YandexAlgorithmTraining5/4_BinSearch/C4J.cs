using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Yandex5._4;

public static class C4J
{
    public static DrawDescription Runner()
    {
        using var file = new StreamReader(@"F:\Projects\Textes\input.txt");
        var nhArr = file.ReadLine()!.Trim().Split().ToList();
        var n = int.Parse(nhArr[0]);
        var h = double.Parse(nhArr[1], NumberStyles.Any, CultureInfo.InvariantCulture);
        var points = Enumerable.Range(0, n).Select(_ =>
        {
            var vecArr = file.ReadLine()!.Trim().Split()
                .Select(v => double.Parse(v, CultureInfo.InvariantCulture)).ToList();
            return new Vector(vecArr[0], vecArr[1]);
        }).ToArray();
        // var result = Solution(h, points);
        return new DrawDescription(points, Array.Empty<Vector>());
    }

    private const double compareAccuracy = 0.000001;

    public static void ConsoleRunner()
    {
        var result = Runner();
    }

    public static ResultDescription Solution(double hDeep, IReadOnlyList<Vector> points)
    {
        var barrels = GetWaterBarrels(hDeep, points);


        return new ResultDescription(1, new List<Vector>());
    }

    private static void FillSomeWater(Barrel[] barrels, IReadOnlyList<Vector> points)
    {
        //indexes of points
        var cups = ConvertToListOfCups(barrels, points);
    }

    private class Cup
    {
        public readonly List<Vector> Points = new();
        public readonly List<Barrel> Barrels = new();
        public bool hasLeftWall;
        public bool hasRightWall;
    }

    private static IReadOnlyList<Cup> ConvertToListOfCups(Barrel[] barrels, IReadOnlyList<Vector> points)
    {
        var cups = new List<Cup>();
        var currentCup = new Cup()
        {
            Points = { points[0] }
        };
        for (var i = 1; i < points.Count - 1; i++)
        {
            var curPoint = points[i];
            currentCup.Barrels.Add(barrels[i - 1]);
            if (curPoint.Y > points[i - 1].Y + compareAccuracy && curPoint.Y > points[i + 1].Y + compareAccuracy)
            {
                currentCup.Points.Add(curPoint);
                cups.Add(currentCup);
                currentCup = new Cup()
                {
                    Points = { curPoint }
                };
            }
        }

        currentCup.Points.Add(points[^1]);
        currentCup.Barrels.Add(barrels[^1]);
        cups.Add(currentCup);

        cups[0].hasLeftWall = true;
        cups[^1].hasRightWall = true;
        return cups;
    }

    public class Barrel
    {
        public double Volume;
    }

    private static Barrel[] GetWaterBarrels(double hDeep, IReadOnlyList<Vector> points)
    {
        var result = new double[points.Count - 1];
        for (var i = 0; i < points.Count - 1; i++)
        {
            result[i] = (points[i + 1].X - points[i].X) * hDeep;
        }

        throw new NotImplementedException();
        //return result;
    }

    public record Vector(double X, double Y)
    {
    }

    public record ResultDescription(double answer, IReadOnlyList<Vector> waterPoints);

    public record DrawDescription(IReadOnlyList<Vector> stonePoints, IReadOnlyList<Vector> waterPoints);
}