using System.Text;

namespace YandexAlgorithmTraining5._2_LinearSearch;

public class C2
{
    public static void A()
    {
        var k = int.Parse(Console.ReadLine()!);
        var coordinates = Enumerable.Range(0, k).Select(_ =>
        {
            var aArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
            return (x: aArr[0], y: aArr[1]);
        }).ToList();

        var xMin = coordinates.Select(c => c.x).Min();
        var xMax = coordinates.Select(c => c.x).Max();
        var yMin = coordinates.Select(c => c.y).Min();
        var yMax = coordinates.Select(c => c.y).Max();

        Console.WriteLine($"{xMin} {yMin} {xMax} {yMax}");
    }

    public static void B()
    {
        var aArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var K = aArr[1];
        var prices = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToArray();
        var maxes = new int[prices.Length];
        var curMaxes = new LinkedList<(int price, int pos)>();
        curMaxes.AddFirst((prices[prices.Length - 1], prices.Length - 1));

        for (int i = prices.Length - 2; i >= 0; i--)
        {
            while (curMaxes.First.Value.pos > i + K)
            {
                curMaxes.RemoveFirst();
            }

            maxes[i] = curMaxes.First.Value.price;

            while (curMaxes.Count > 0 && curMaxes.Last!.Value.price < prices[i])
            {
                curMaxes.RemoveLast();
            }

            curMaxes.AddLast((prices[i], i));
        }

        int? result = null;
        for (int i = 0; i < prices.Length - 1; i++)
        {
            var diff = maxes[i] - prices[i];
            if (diff > 0 && (result is null || result.Value < diff))
            {
                result = diff;
            }
        }

        Console.WriteLine(result ?? 0);
    }

    public static void C()
    {
        var N = int.Parse(Console.ReadLine()!);
        var lenghts = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var max = lenghts.Max();
        if (lenghts.Count(l => l == max) == 1 && (max - lenghts.Where(l => l != max).Sum()) is var posLen && posLen > 0)
        {
            Console.WriteLine(posLen);
        }
        else
        {
            Console.WriteLine(lenghts.Sum());
        }
    }

    public static void D()
    {
        var n = int.Parse(Console.ReadLine()!);
        var cells = Enumerable.Range(0, n)
            .Select(_ => Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList())
            .Select(i => (x: i[0], y: i[1])).ToList();
        var indexedCells = cells.ToHashSet();
        var deltas = new List<(int x, int y)>() { (1, 0), (0, -1), (-1, 0), (0, 1) };
        var result = cells.Select(c => 4 - deltas.Count(d => indexedCells.Contains((c.x + d.x, c.y + d.y)))).Sum();
        Console.WriteLine(result);
    }

    public static void E()
    {
        var n = int.Parse(Console.ReadLine()!);
        var berries = Enumerable.Range(0, n)
            .Select(i => Console.ReadLine()!.Trim().Split().Select(long.Parse).ToList())
            .Select((list, num) => new Berry(num, list[0], list[1])).ToList();

        var before = new List<Berry>();
        var after = new List<Berry>();

        void placeBeforeOrAfter(Berry curBerry)
        {
            if (curBerry.day > curBerry.night)
            {
                before.Add(curBerry);
            }
            else
            {
                after.Add(curBerry);
            }
        }

        for (int i = 0; i < berries.Count; i++)
        {
            placeBeforeOrAfter(berries[i]);
        }

        if (before.Count > 0)
        {
            var worst = before.MaxBy(b => b.night)!;
            before.Remove(worst);
            before.Add(worst);
        }

        if (after.Count > 0)
        {
            var best = after.MaxBy(b => b.day);
            after.Remove(best);
            after.Insert(0, best);
        }

        long resMax = 0;

        if (after.Any() && before.Any())
        {
            resMax = Math.Max(before.Sum(b => b.day - b.night) + after[0].day,
                before.Take(before.Count - 1).Sum(b => b.day - b.night) + before[^1].day);
        }
        else if (before.Any())
        {
            resMax = before.Take(before.Count - 1).Sum(b => b.day - b.night) + before[^1].day;
        }
        else
        {
            resMax = after[0].day;
        }

        Console.WriteLine(resMax);

        // var maxHeigh = before.Sum(b => b.day + b.night) + center.day;
        //Console.WriteLine(maxHeigh);
        var res = string.Join(" ", before.Concat(after).Select(b => b.number + 1));
        Console.WriteLine(res);
    }

    record Berry(long number, long day, long night);

    public static void F()
    {
        long mod(long dividable, long divider)
        {
            var result = dividable = dividable % divider;
            return result >= 0 ? result : result + divider;
        }

        var n = int.Parse(Console.ReadLine()!);
        var sectors = Console.ReadLine()!.Trim().Split().Select(long.Parse).ToArray();
        var abkArr = Console.ReadLine()!.Trim().Split().Select(long.Parse).ToList();
        var a = abkArr[0];
        var b = abkArr[1];
        var k = abkArr[2];

        var aSectors = (a - 1) / k;
        var bSectors = (b - 1) / k;

        var canHaveSectorsright = new bool[sectors.Length];
        var canHaveSectorsLeft = new bool[sectors.Length];
        long? maxOutcome = null;

        for (long i = aSectors; i <= bSectors; i++)
        {
            var sectorNum = i % sectors.Length;
            if (canHaveSectorsright[sectorNum])
            {
                break;
            }

            canHaveSectorsright[sectorNum] = true;

            if (maxOutcome is null || maxOutcome.Value < sectors[sectorNum])
            {
                maxOutcome = sectors[sectorNum];
            }
        }

        for (long i = aSectors; i <= bSectors; i++)
        {
            var sectorNum = mod(-i, sectors.Length);
            if (canHaveSectorsLeft[sectorNum])
            {
                break;
            }

            canHaveSectorsLeft[sectorNum] = true;

            if (maxOutcome is null || maxOutcome.Value < sectors[sectorNum])
            {
                maxOutcome = sectors[sectorNum];
            }
        }

        Console.WriteLine(maxOutcome!.Value);
    }


    public static void G()
    {
        var tLen = int.Parse(Console.ReadLine()!);
        var resSb = new StringBuilder();
        for (int ttt = 0; ttt < tLen; ttt++)
        {
            var len = int.Parse(Console.ReadLine()!);
            var numbers = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();

            var result = new List<Segment>() { new Segment() { Count = 1, MinNumber = numbers[0] } };

            for (int i = 1; i < numbers.Count; i++)
            {
                var lastSegment = result[^1];
                if (lastSegment.Count + 1 <= numbers[i] && lastSegment.Count + 1 <= lastSegment.MinNumber)
                {
                    lastSegment.MinNumber = Math.Min(lastSegment.MinNumber, numbers[i]);
                    lastSegment.Count++;
                }
                else
                {
                    result.Add(new Segment() { Count = 1, MinNumber = numbers[i] });
                }
            }

            resSb.AppendLine(result.Count.ToString());
            resSb.AppendLine(string.Join(' ', result.Select(s => s.Count)));
            // Console.WriteLine(result.Count);
            // Console.WriteLine(string.Join(' ', result.Select(s => s.Count)));
        }

        Console.WriteLine(resSb.ToString());
    }

    class Segment
    {
        public int MinNumber;
        public int Count;
    }

    public static void H()
    {
        var nmArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var n = nmArr[0];
        var m = nmArr[1];

        var classRaces = Enumerable.Range(0, n)
            .Select(_ => Console.ReadLine()!.Trim().Split().Select(int.Parse).ToArray()).ToArray();

        var allCombinations =
            classRaces.SelectMany((races, clss) =>
                races.Select((pow, r) => new Combination(pow: pow, clss, race: r))).ToList();

        allCombinations.Sort(Comparer<Combination>.Create((a, b) => -a.pow.CompareTo(b.pow)));

        var var1Class = allCombinations[0].clss;
        var var1Index = allCombinations.FindIndex(comb => comb.clss != allCombinations[0].clss);

        var var1Race = var1Index == -1 ? 0 : allCombinations[var1Index].race;

        var var2Race = allCombinations[0].race;
        var var2Index = allCombinations.FindIndex(comb => comb.race != allCombinations[0].race);
        var var2Class = var2Index == -1 ? 0 : allCombinations[var2Index].clss;

        if (var1Index == -1 || var2Index == -1)
        {
            Console.WriteLine(var1Index == -1 ? $"{var1Class + 1} {var1Race + 1}" : $"{var2Class + 1} {var2Race + 1}");
        }

        var var1MaxLeftPowah = allCombinations.First(comb => comb.clss != var1Class && comb.race != var1Race).pow;
        var var2MaxLeftPowah = allCombinations.First(comb => comb.clss != var2Class && comb.race != var2Race).pow;
        Console.WriteLine(var1MaxLeftPowah < var2MaxLeftPowah
            ? $"{var1Class + 1} {var1Race + 1}"
            : $"{var2Class + 1} {var2Race + 1}");
    }

    record Combination(int pow, int clss, int race);

    public static void I()
    {
        var directions = new int[] { -1, 1 };
        var N = int.Parse(Console.ReadLine()!);
        var ships = Enumerable.Range(0, N).Select(_ => Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList())
            .Select(l => new IntVec(l[1] - 1, l[0] - 1)).ToList();
        var originalMatrix = new bool[N][];
        for (int i = 0; i < N; i++)
        {
            originalMatrix[i] = new bool[N];
        }

        foreach (var ship in ships)
        {
            originalMatrix[ship.col][ship.row] = true;
        }

        int? finalResult = null;

        for (int targCol = 0; targCol < N; targCol++)
        {
            var currentMatrix = originalMatrix.Select(c => c.ToArray()).ToArray();
            var movedToGetToThisCell = new int[N];
            int currentResult = 0;
            for (int colDelta = 1; colDelta < N; colDelta++)
            {
                foreach (var direction in directions)
                {
                    if (targCol + direction * colDelta is var colFrom && colFrom >= 0 && colFrom < N)
                    {
                        while (currentMatrix[colFrom].Any(r => r))
                        {
                            var distances = new int?[N];
                            var targets = new int[N];
                            for (int jFromRow = 0; jFromRow < N; jFromRow++)
                            {
                                if (currentMatrix[colFrom][jFromRow])
                                {
                                    for (int jTargRow = 0; jTargRow < N; jTargRow++)
                                    {
                                        if (currentMatrix[targCol][jTargRow] is false &&
                                            Math.Abs(jTargRow - jFromRow) is var dist &&
                                            (distances[jFromRow] is null || dist < distances[jFromRow]!.Value))
                                        {
                                            distances[jFromRow] = dist;
                                            targets[jFromRow] = jTargRow;
                                        }
                                    }
                                }
                            }

                            var rowMinDist = IndexOf(distances, dist => dist == distances.Min());
                            currentMatrix[colFrom][rowMinDist] = false;
                            currentMatrix[targCol][targets[rowMinDist]] = true;
                            var curDist = distances[rowMinDist]!.Value + colDelta;
                            movedToGetToThisCell[targets[rowMinDist]] = curDist;
                            currentResult += curDist;
                        }
                    }
                }
            }

            if (finalResult is null || finalResult > currentResult)
            {
                finalResult = currentResult;
            }
        }

        Console.WriteLine(finalResult);
    }

    public static void I2()
    {
        var N = int.Parse(Console.ReadLine()!);
        var ships = Enumerable.Range(0, N).Select(_ => Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList())
            .Select(l => new IntVec(l[1] - 1, l[0] - 1)).ToList();

        var originalMatrix = new bool[N][];
        for (int i = 0; i < N; i++)
        {
            originalMatrix[i] = new bool[N];
        }

        foreach (var ship in ships)
        {
            originalMatrix[ship.col][ship.row] = true;
        }

        int? finalResult = null;

        for (int targCol = 0; targCol < N; targCol++)
        {
            var groupedShips = ships.GroupBy(s => Math.Abs(targCol - s.col)).Select(g => (g.Key, g.ToList()))
                .OrderBy(g => g.Key)
                .ToList();

            var movedToGetToThisCell = new int[N];
            int currentResult = 0;

            var centralColumn = originalMatrix[targCol];

            foreach (var shipGroup in groupedShips.Where(g => g.Key > 0))
            {
                var colShips = shipGroup.Item2.ToList();
                while (colShips.Count > 0)
                {
                    var colShipsVertDistances = new int[colShips.Count];
                    var targets = new int[colShips.Count];
                    for (int fromIndex = 0; fromIndex < colShips.Count; fromIndex++)
                    {
                        int? curColShipVertDistance = null;
                        int target = -1;
                        for (int toRow = 0; toRow < N; toRow++)
                        {
                            if (centralColumn[toRow] is false &&
                                Math.Abs(colShips[fromIndex].row - toRow) is var vertDistHere &&
                                (curColShipVertDistance is null || vertDistHere < curColShipVertDistance))
                            {
                                curColShipVertDistance = vertDistHere;
                                target = toRow;
                            }
                        }

                        colShipsVertDistances[fromIndex] = curColShipVertDistance!.Value;
                        targets[fromIndex] = target;
                    }

                    var nearestShipIndex = IndexOf(colShipsVertDistances, v => v == colShipsVertDistances.Min());
                    var curDist = colShipsVertDistances[nearestShipIndex] + shipGroup.Key;
                    currentResult += curDist;
                    colShips.Remove(colShips[nearestShipIndex]);
                }
            }


            if (finalResult is null || finalResult > currentResult)
            {
                finalResult = currentResult;
            }
        }

        Console.WriteLine(finalResult);
    }


    public static void I3()
    {
        var N = int.Parse(Console.ReadLine()!);
        var ships = Enumerable.Range(0, N).Select(_ => Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList())
            .Select(l => new IntVec(l[1] - 1, l[0] - 1)).ToList();
        var countOfStepsToSpreadVertically = 0;
        var shipsToSpreadVertically = new HashSet<IntVec>(ships);
        //place a ship to this row
        for (int i = 0; i < N; i++)
        {
            var ship = shipsToSpreadVertically.MinBy(s => s.row - i)!;
            countOfStepsToSpreadVertically += Math.Abs(ship.row - i);
            shipsToSpreadVertically.Remove(ship);
        }

        int? countOfStepsToSpreadHorizontally = null;
        //enumerate through all columns
        for (int colToPlace = 0; colToPlace < N; colToPlace++)
        {
            var currentCountOfSteps = ships.Select(s => Math.Abs(colToPlace - s.col)).Sum();
            if (countOfStepsToSpreadHorizontally is null ||
                currentCountOfSteps < countOfStepsToSpreadHorizontally.Value)
            {
                countOfStepsToSpreadHorizontally = currentCountOfSteps;
            }
        }


        // var usedColShips = new HashSet<IntVec>();
        // //place a ship to this column
        // for (int i = 0; i < N; i++)
        // {
        //     var ship = ships.Where(s => !usedColShips.Contains(s)).MinBy(s => Math.Abs(s.col - i))!;
        //     countOfStepsToSpreadHorizontally += Math.Abs(ship.col - i);
        //     usedColShips.Add(ship);
        // }

        Console.WriteLine(countOfStepsToSpreadVertically + countOfStepsToSpreadHorizontally);
    }

    public static int IndexOf<T>(IEnumerable<T> obj, Func<T, bool> predicate)
    {
        var found = obj
            .Select((a, i) => new { a, i })
            .FirstOrDefault(x => predicate(x.a));
        return found?.i ?? -1;
    }

    record IntVec(int col, int row);

    public static void J()
    {
        var mnArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var H = mnArr[0];
        var W = mnArr[1];
        var matrix = Enumerable.Range(0, H).Select(_ => Console.ReadLine()!.Trim().ToArray()).ToArray();

        (Vec topLeft, Vec downRight)? FillWithLetter(char[][] arr, char letter)
        {
            var iup = 0;
            var jleft = 0;
            while (iup < H && arr[iup][jleft] != '#')
            {
                jleft++;
                if (jleft >= W)
                {
                    jleft = 0;
                    iup++;
                }
            }

            if (iup == H)
            {
                return null;
            }

            var jr = jleft;
            while (jr < W && arr[iup][jr] == '#')
            {
                jr++;
            }

            var idown = iup;
            while (idown < H && arr[idown][jleft..(jr)].All(c => c == '#'))
            {
                idown++;
            }

            for (int i = iup; i < idown; i++)
            {
                for (int j = jleft; j < jr; j++)
                {
                    arr[i][j] = letter;
                }
            }

            return (new Vec(iup, jleft), new Vec(idown, jr));
        }

        var aCoordinates = FillWithLetter(matrix, 'a');
        var bCoordinates = FillWithLetter(matrix, 'b');

        var countOfA = matrix.SelectMany(c => c).Count(c => c == 'a');
        var countOfB = matrix.SelectMany(c => c).Count(c => c == 'b');
        var countOfSharp = matrix.SelectMany(c => c).Count(c => c == '#');

        if (countOfSharp > 0)
        {
            Console.WriteLine("NO");
            return;
        }

        if (countOfB > 0)
        {
            Console.WriteLine("YES");
            for (int i = 0; i < H; i++)
            {
                Console.WriteLine(string.Join("", matrix[i]));
            }

            return;
        }

        if (countOfB == 0 && countOfA <= 1)
        {
            Console.WriteLine("NO");
            return;
        }
        //countOfB == 0 && countOfA > 1

        if (aCoordinates!.Value.downRight.ii - aCoordinates!.Value.topLeft.ii > 1)
        {
            for (int j = aCoordinates.Value.topLeft.jj; j < aCoordinates!.Value.downRight.jj; j++)
            {
                matrix[aCoordinates!.Value.topLeft.ii][j] = 'b';
            }
        }
        else
        {
            for (int i = aCoordinates!.Value.topLeft.ii; i < aCoordinates.Value.downRight.ii; i++)
            {
                matrix[i][aCoordinates.Value.topLeft.jj] = 'b';
            }
        }

        Console.WriteLine("YES");
        for (int i = 0; i < H; i++)
        {
            Console.WriteLine(string.Join("", matrix[i]));
        }
    }

    public static void J2()
    {
        var mnArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var H = mnArr[0];
        var W = mnArr[1];
        var originalMatrix = Enumerable.Range(0, H).Select(_ => Console.ReadLine()!.Trim().ToArray()).ToArray();
        var rotated = originalMatrix;

        for (int cntOfRotates = 0; cntOfRotates < 4; cntOfRotates++)
        {
            var result = SolveMatrixAngle(rotated);
            if (result is not null)
            {
                for (int i = 0; i < (4 - cntOfRotates) % 4; i++)
                {
                    result = RotateClockWise(result);
                }

                Console.WriteLine("YES");
                for (int i = 0; i < H; i++)
                {
                    Console.WriteLine(string.Join("", result[i]));
                }

                return;
            }

            rotated = RotateClockWise(rotated);
        }

        Console.WriteLine("NO");
    }

    static char[][] RotateClockWise(char[][] matrix)
    {
        var result = Enumerable.Range(0, matrix[0].Length)
            .Select(ind => Enumerable.Range(0, matrix.Length).Select(jnd => matrix[^(jnd + 1)][ind]).ToArray())
            .ToArray();
        return result;
    }

    static char[][]? SolveMatrixAngle(char[][] originalMatrix)
    {
        var matrix = originalMatrix.Select(m => m.ToArray()).ToArray();
        var H = matrix.Length;
        var W = matrix[0].Length;

        (Vec topLeft, Vec downRight)? FillWithLetter(char[][] arr, char letter)
        {
            var iup = 0;
            var jleft = 0;
            while (iup < H && arr[iup][jleft] != '#')
            {
                jleft++;
                if (jleft >= W)
                {
                    jleft = 0;
                    iup++;
                }
            }

            if (iup == H)
            {
                return null;
            }

            var jr = jleft;
            while (jr < W && arr[iup][jr] == '#')
            {
                jr++;
            }

            var idown = iup;
            while (idown < H && arr[idown][jleft..(jr)].All(c => c == '#'))
            {
                idown++;
            }

            for (int i = iup; i < idown; i++)
            {
                for (int j = jleft; j < jr; j++)
                {
                    arr[i][j] = letter;
                }
            }

            return (new Vec(iup, jleft), new Vec(idown, jr));
        }

        var aCoordinates = FillWithLetter(matrix, 'a');
        var bCoordinates = FillWithLetter(matrix, 'b');

        var countOfA = matrix.SelectMany(c => c).Count(c => c == 'a');
        var countOfB = matrix.SelectMany(c => c).Count(c => c == 'b');
        var countOfSharp = matrix.SelectMany(c => c).Count(c => c == '#');

        if (countOfSharp > 0)
        {
            return null;
        }

        if (countOfB > 0)
        {
            return matrix;
        }

        if (countOfB == 0 && countOfA <= 1)
        {
            return null;
        }

        if (aCoordinates!.Value.downRight.ii - aCoordinates!.Value.topLeft.ii > 1)
        {
            for (int j = aCoordinates.Value.topLeft.jj; j < aCoordinates!.Value.downRight.jj; j++)
            {
                matrix[aCoordinates!.Value.topLeft.ii][j] = 'b';
            }
        }
        else
        {
            for (int i = aCoordinates!.Value.topLeft.ii; i < aCoordinates.Value.downRight.ii; i++)
            {
                matrix[i][aCoordinates.Value.topLeft.jj] = 'b';
            }
        }

        return matrix;
    }

    record Vec(int ii, int jj);
}