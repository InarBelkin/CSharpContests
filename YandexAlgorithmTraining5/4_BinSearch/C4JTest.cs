// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
//
// namespace Yandex5._4;
//
// using static C4J2;
//
// public static class C4JTest
// {
//     public static void TestDeep()
//     {
//         using var file = new StreamReader(@"F:\Projects\Textes\input.txt");
//         var nhArr = file.ReadLine()!.Trim().Split().ToList();
//         var n = int.Parse(nhArr[0]);
//         var h = double.Parse(nhArr[1], CultureInfo.InvariantCulture);
//
//         var points = Enumerable.Range(0, n + 1).Select(_ =>
//         {
//             var st = file.ReadLine()!;
//             var vecArr = st!.Trim().Split()
//                 .Select(v => double.Parse(v.Replace('–', '-'), CultureInfo.InvariantCulture)).ToList();
//             return new Vec(vecArr[0], vecArr[1]);
//         }).ToList();
//         points.Add(new Vec(points[^1].X, 100000));
//
//         for (int i = 0; i < points.Count - 1; i++)
//         {
//             if (i % 2 == 0)
//             {
//                 if (points[i].Y < points[i + 1].Y)
//                 {
//                     throw new Exception("WTF");
//                 }
//             }
//             else
//             {
//                 if (points[i].Y > points[i + 1].Y)
//                 {
//                     throw new Exception("WTF");
//                 }
//             }
//         }
//
//         var heights = new List<(int index, double height, double mHeight)>();
//         for (int i = 0; i < points.Count - 1; i += 2)
//         {
//             var minLR = Math.Min(points[i].Y, points[i + 2].Y);
//             var targWaterArea = h * (points[i + 2].X - points[i].X);
//             var maxWaterArea = (points[i + 2].X - points[i].X) / 2 * (minLR - points[i + 1].Y);
//
//             var waterHeight = binSearch(0, (minLR - points[i + 1].Y), m =>
//             {
//                 var topLeft = GetPointAtY(points[i], points[i + 1], points[i + 1].Y + m);
//                 var toRight = GetPointAtY(points[i + 2], points[i + 1], points[i + 1].Y + m);
//                 var curArea = (toRight.X - topLeft.X) * m / 2;
//                 return curArea >= targWaterArea;
//             });
//             heights.Add((i + 1, waterHeight, minLR));
//         }
//
//         Console.WriteLine(heights.MaxBy(h => h.height));
//     }
//
//     private static double binSearch(double l, double r, Func<double, bool> check)
//     {
//         while (r - l >= precision)
//         {
//             var m = (l + r) / 2;
//             if (check(m))
//             {
//                 r = m;
//             }
//             else
//             {
//                 l = m;
//             }
//         }
//
//         return (l + r) / 2;
//     }
//
//     private static Vec GetPointAtY(Vec top, Vec down, double y)
//     {
//         var x = ((y - down.Y) / (top.Y - down.Y)) * (top.X - down.X) + down.X;
//         return new Vec(x, y);
//     }
// }