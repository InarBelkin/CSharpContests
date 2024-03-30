// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
//
// namespace Yandex5._4;
//
// public static class C4J2
// {
//     public static List<TrapezToDraw> TrapeziesToDraw = new List<TrapezToDraw>();
//
//     public static void StartWithRead()
//     {
//         using var file = new StreamReader(@"F:\Projects\Textes\input.txt");
//         var nhArr = file.ReadLine()!.Trim().Split().ToList();
//         var n = int.Parse(nhArr[0]);
//         var h = double.Parse(nhArr[1], CultureInfo.InvariantCulture);
//
//         var points = Enumerable.Range(0, n + 1).Select(_ =>
//         {
//             var st = file.ReadLine()!;
//             if (!st.All(c => "0123456789 -".Contains(c)))
//             {
//                 throw new Exception("WTF");
//             }
//
//             var vecArr = st!.Trim().Split()
//                 .Select(v => double.Parse(v.Replace('–', '-'), CultureInfo.InvariantCulture)).ToList();
//             return new Vec(vecArr[0], vecArr[1]);
//         }).ToArray();
//
//         // var maxX = points.Select(p => p.X).Max();
//         // points = points.Reverse().Select(p => new Vec(maxX - p.X, p.Y)).ToArray();
//         var result = Solution(h, points);
//         Console.WriteLine(result.ToString(CultureInfo.InvariantCulture));
//     }
//
//     public static double Solution(double hDeep, IReadOnlyList<Vec> points)
//     {
//         if (hDeep == 0)
//         {
//             return 0;
//         }
//
//         var maxY = points.Select(p => p.Y).MaxBy(y => y);
//         // var bigTrapeze = AddTrapeziesUnderneath(null, points, new Vec(points[0].X, maxY + hDeep),
//         //     new Vec(points[^1].X, maxY + hDeep + 1));
//         var bigTrapeze = AddAnotherTrap(points, new Vec(points[0].X, maxY + hDeep),
//             new Vec(points[^1].X, maxY + hDeep + 1));
//
//         var sumAreaOfWater = hDeep * (points[^1].X - points[0].X);
//         var (result, waterArea) = SolveTrapeze(bigTrapeze, sumAreaOfWater);
//         return result;
//     }
//
//     // private static double SolveTrapeze(Trapeze trapeze, double sumAreaOfWater)
//     // {
//     //     if (trapeze.SumAreaUnderneath >= sumAreaOfWater)
//     //     {
//     //         if (trapeze.Children.Count == 1)
//     //         {
//     //             return SolveTrapeze(trapeze.Children[0], sumAreaOfWater);
//     //         }
//     //         else
//     //         {
//     //             var trap0Len = GetTopLen(trapeze.Children[0]);
//     //             var trap1Len = GetTopLen(trapeze.Children[1]);
//     //
//     //             var trap0Proportion = trap0Len / (trap0Len + trap1Len);
//     //             var trap1Proportion = trap1Len / (trap0Len + trap1Len);
//     //
//     //             var trap0Water = sumAreaOfWater * trap0Proportion;
//     //             var trap1Water = sumAreaOfWater * trap1Proportion;
//     //
//     //             var trap0SumArea = trapeze.Children[0].Area + trapeze.Children[0].SumAreaUnderneath;
//     //             var trap1SumArea = trapeze.Children[1].Area + trapeze.Children[1].SumAreaUnderneath;
//     //
//     //             double trap0MaxDeep;
//     //             double trap1MaxDeep;
//     //
//     //             if (trap0Water > trap0SumArea)
//     //             {
//     //                 trap0MaxDeep = SolveTrapeze(trapeze.Children[0], trap0SumArea);
//     //                 trap1MaxDeep = SolveTrapeze(trapeze.Children[1], sumAreaOfWater - trap0SumArea);
//     //             }
//     //             else if (trap1Water > trap1SumArea)
//     //             {
//     //                 trap0MaxDeep = SolveTrapeze(trapeze.Children[0], sumAreaOfWater - trap1SumArea);
//     //                 trap1MaxDeep = SolveTrapeze(trapeze.Children[1], trap1SumArea);
//     //             }
//     //             else
//     //             {
//     //                 trap0MaxDeep = SolveTrapeze(trapeze.Children[0], trap0Water);
//     //                 trap1MaxDeep = SolveTrapeze(trapeze.Children[1], trap1Water);
//     //             }
//     //
//     //             return Math.Max(trap0MaxDeep, trap1MaxDeep);
//     //         }
//     //     }
//     //
//     //     var currentWaterArea = sumAreaOfWater - trapeze.SumAreaUnderneath;
//     //     var currentHeigh = binSearch(0, trapeze.A.Y - trapeze.C.Y, m =>
//     //     {
//     //         var areaOfTrapeze = CalcArea(trapeze, m);
//     //         return areaOfTrapeze >= currentWaterArea;
//     //     });
//     //     return (currentHeigh + trapeze.C.Y) - trapeze.MinY;
//     // }
//
//     private static (double result, double waterArea) SolveTrapeze(Trapeze trapeze, double sumAreaOfWater)
//     {
//         if (trapeze.SumAreaUnderneath > sumAreaOfWater)
//         {
//             if (trapeze.Children.Count == 1)
//             {
//                 return SolveTrapeze(trapeze.Children[0], sumAreaOfWater);
//             }
//             else
//             {
//                 var trap0Len = GetTopLen(trapeze.Children[0]);
//                 var trap1Len = GetTopLen(trapeze.Children[1]);
//
//                 var trap0Proportion = trap0Len / (trap0Len + trap1Len);
//                 var trap1Proportion = trap1Len / (trap0Len + trap1Len);
//
//                 var trap0Water = sumAreaOfWater * trap0Proportion;
//                 var trap1Water = sumAreaOfWater * trap1Proportion;
//
//                 var trap0SumArea = trapeze.Children[0].Area + trapeze.Children[0].SumAreaUnderneath;
//                 var trap1SumArea = trapeze.Children[1].Area + trapeze.Children[1].SumAreaUnderneath;
//
//                 double trap0MaxDeep;
//                 double trap1MaxDeep;
//                 double trap0WArea;
//                 double trap1WArea;
//
//                 if (trap0Water > trap0SumArea)
//                 {
//                     (trap0MaxDeep, trap0WArea) = SolveTrapeze(trapeze.Children[0], trap0SumArea);
//                     (trap1MaxDeep, trap1WArea) = SolveTrapeze(trapeze.Children[1], sumAreaOfWater - trap0SumArea);
//                 }
//                 else if (trap1Water > trap1SumArea)
//                 {
//                     (trap0MaxDeep, trap0WArea) = SolveTrapeze(trapeze.Children[0], sumAreaOfWater - trap1SumArea);
//                     (trap1MaxDeep, trap1WArea) = SolveTrapeze(trapeze.Children[1], trap1SumArea);
//                 }
//                 else
//                 {
//                     (trap0MaxDeep, trap0WArea) = SolveTrapeze(trapeze.Children[0], trap0Water);
//                     (trap1MaxDeep, trap1WArea) = SolveTrapeze(trapeze.Children[1], trap1Water);
//                 }
//
//                 var maxDeep = Math.Max(trap0MaxDeep, trap1MaxDeep);
//                 var wArea = trap0WArea + trap1WArea;
//
//                 return (maxDeep, wArea);
//                 //  return Math.Max(trap0MaxDeep, trap1MaxDeep);
//             }
//         }
//
//         var currentWaterArea = sumAreaOfWater - trapeze.SumAreaUnderneath;
//         var currentHeigh = binSearch(0, trapeze.A.Y - trapeze.C.Y, m =>
//         {
//             var areaOfTrapeze = CalcArea(trapeze, m);
//             return areaOfTrapeze >= currentWaterArea;
//         });
//         if (currentHeigh < 0)
//             throw new Exception("WTF");
//         var curArea = CalcArea(trapeze, currentHeigh);
//         AddTrapezToDraw(trapeze, currentHeigh);
//
//         return ((currentHeigh + trapeze.C.Y) - trapeze.MinY, curArea + trapeze.SumAreaUnderneath);
//     }
//
//
//     private static Trapeze AddAnotherTrap(IReadOnlyList<Vec> points, Vec leftTop, Vec rightTop)
//     {
//         var trapeze = new Trapeze(new Vec(leftTop.X, leftTop.Y + 10000), new Vec(rightTop.X, rightTop.Y + 10000),
//             leftTop,
//             rightTop);
//         var child = AddTrapeziesUnderneath(trapeze, points, leftTop, rightTop);
//         trapeze.Children.Add(child);
//         trapeze.SumAreaUnderneath = child.SumAreaUnderneath + child.Area;
//         trapeze.MinY = child.MinY;
//         return trapeze;
//     }
//
//     private static Trapeze AddTrapeziesUnderneath(Trapeze? parent, IReadOnlyList<Vec> points, Vec leftTop, Vec rightTop)
//     {
//         if (points.Count == 1)
//         {
//             var lasttrapeze = new Trapeze(leftTop, rightTop, points[0], points[0])
//             {
//                 Parent = parent,
//                 SumAreaUnderneath = 0,
//                 MinY = points[0].Y,
//             };
//             return lasttrapeze;
//         }
//
//         Trapeze trapeze;
//         var maxIndex = GetIndexOfMax(points);
//         if (points[maxIndex].Y >= leftTop.Y)
//         {
//             throw new Exception("WTF");
//         }
//
//         if (maxIndex == 0)
//         {
//             trapeze = new Trapeze(
//                 leftTop,
//                 rightTop,
//                 points[0],
//                 GetPointAtY(rightTop, points[^1], points[0].Y));
//             trapeze.Children.Add(AddTrapeziesUnderneath(trapeze, points.Skip(1).ToList(), trapeze.C, trapeze.D));
//         }
//         else if (maxIndex == points.Count - 1)
//         {
//             trapeze = new Trapeze(
//                 leftTop,
//                 rightTop,
//                 GetPointAtY(leftTop, points[0], points[^1].Y),
//                 points[^1]
//             );
//             trapeze.Children.Add(AddTrapeziesUnderneath(trapeze, points.Take(points.Count - 1).ToList(), trapeze.C,
//                 trapeze.D));
//         }
//         else
//         {
//             trapeze = new Trapeze(
//                 leftTop,
//                 rightTop,
//                 GetPointAtY(leftTop, points[0], points[maxIndex].Y),
//                 GetPointAtY(rightTop, points[^1], points[maxIndex].Y));
//             trapeze.Children.Add(
//                 AddTrapeziesUnderneath(trapeze, points.Take(maxIndex).ToList(), trapeze.C, points[maxIndex]));
//             trapeze.Children.Add(
//                 AddTrapeziesUnderneath(trapeze, points.Skip(maxIndex + 1).ToList(), points[maxIndex], trapeze.D));
//         }
//
//         trapeze.Parent = parent;
//         trapeze.SumAreaUnderneath = trapeze.Children.Sum(t => t.Area + t.SumAreaUnderneath);
//         trapeze.MinY = trapeze.Children.Min(t => t.MinY);
//         return trapeze;
//     }
//
//     private record struct AddingResult(Trapeze Trapeze);
//
//     public class Trapeze
//     {
//         public Trapeze(Vec a, Vec b, Vec c, Vec d)
//         {
//             A = a;
//             B = b;
//             C = c;
//             D = d;
//             Area = ((B.X - A.X) + (D.X - C.X)) / 2 * (A.Y - C.Y);
//         }
//
//         public readonly Vec A;
//         public readonly Vec B;
//         public readonly Vec C;
//         public readonly Vec D;
//         public readonly double Area;
//         public double SumAreaUnderneath;
//         public double MinY;
//         public Trapeze? Parent;
//         public readonly List<Trapeze> Children = new();
//     }
//
//     public record Vec(double X, double Y)
//     {
//     }
//
//     private static int GetIndexOfMax(IReadOnlyList<Vec> points)
//     {
//         var max = points[0];
//         var maxIndex = 0;
//         for (var i = 1; i < points.Count; i++)
//         {
//             if (points[i].Y > max.Y)
//             {
//                 max = points[i];
//                 maxIndex = i;
//             }
//         }
//
//         return maxIndex;
//     }
//
//     private static Vec GetPointAtY(Vec top, Vec down, double y)
//     {
//         var x = ((y - down.Y) / (top.Y - down.Y)) * (top.X - down.X) + down.X;
//         return new Vec(x, y);
//     }
//
//     private static double GetTopLen(Trapeze trapeze)
//     {
//         return trapeze.B.X - trapeze.A.X;
//     }
//
//     public static double CalcArea(Trapeze trapeze)
//     {
//         return trapeze.Area + trapeze.Children.Select(c => CalcArea(c)).Sum();
//     }
//
//     public static void SomeTest()
//     {
//         var result = GetPointAtY(new Vec(3, 5), new Vec(4, 2), 4);
//         Console.WriteLine(result.X);
//     }
//
//     public const double precision = 0.00001;
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
//     private static double CalcArea(Trapeze trapeze, double h)
//     {
//         var hY = trapeze.C.Y + h;
//         var hA = GetPointAtY(trapeze.A, trapeze.C, hY);
//         var hB = GetPointAtY(trapeze.B, trapeze.D, hY);
//         var topLen = hB.X - hA.X;
//         var downLen = trapeze.D.X - trapeze.C.X;
//         return (topLen + downLen) / 2 * (h);
//     }
//
//     private static void AddTrapezToDraw(Trapeze trapeze, double waterLevel)
//     {
//         var topLeftPoint = GetPointAtY(trapeze.A, trapeze.C, trapeze.C.Y + waterLevel);
//         var toRightPoint = GetPointAtY(trapeze.B, trapeze.D, trapeze.C.Y + waterLevel);
//         TrapeziesToDraw.Add(new TrapezToDraw(toVector(topLeftPoint), toVector(toRightPoint), toVector(trapeze.C),
//             toVector(trapeze.D)));
//     }
//
//     private static C4J.Vector toVector(Vec vec)
//     {
//         return new C4J.Vector(vec.X, vec.Y);
//     }
//
//     public record TrapezToDraw(C4J.Vector A, C4J.Vector B, C4J.Vector C, C4J.Vector D);
// }