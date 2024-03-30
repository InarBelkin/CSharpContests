using System.Text.RegularExpressions;

namespace YandexAlgorithmTraining5._1_Common;

public static class C1
{
    public static void A()
    {
        var aArr = Console.ReadLine()!.Split().Select(int.Parse).ToList();
        var bArr = Console.ReadLine()!.Split().Select(int.Parse).ToList();

        var aTree = aArr[0];
        var aDist = aArr[1];

        var bTree = bArr[0];
        var bDist = bArr[1];

        var aLeft = aTree - aDist;
        var aRight = aTree + aDist;

        var bLeft = bTree - bDist;
        var bRight = bTree + bDist;

        if (bLeft < aLeft)
        {
            (aLeft, bLeft) = (bLeft, aLeft);
            (aRight, bRight) = (bRight, aRight);
        }

        if (bLeft > aRight)
        {
            Console.WriteLine(aRight - aLeft + 1 + bRight - bLeft + 1);
        }
        else
        {
            Console.WriteLine(Math.Max(aRight, bRight) - aLeft + 1);
        }
    }

    public static void B()
    {
        var aArr = Console.ReadLine()!.Trim().Split(':').Select(int.Parse).ToList();
        var bArr = Console.ReadLine()!.Trim().Split(':').Select(int.Parse).ToList();
        var whoIs = int.Parse(Console.ReadLine()!);

        var firstGoals = aArr[0] + bArr[0];
        var secondGoals = aArr[1] + bArr[1];

        var diff = secondGoals - firstGoals;


        if (diff >= 0)
        {
            var firstGuestGoals = whoIs == 1 ? bArr[0] + diff : aArr[0];
            var secondGuestGoals = whoIs == 1 ? aArr[1] : bArr[1];
            Console.WriteLine(firstGuestGoals > secondGuestGoals ? diff : diff + 1);
        }
        else
        {
            Console.WriteLine(0);
        }
    }

    public static void C()
    {
        var tt = long.Parse(Console.ReadLine()!);

        long result = 0;
        for (int ttt = 0; ttt < tt; ttt++)
        {
            var num = long.Parse(Console.ReadLine()!);
            var n = num / 4;
            var part = num % 4 == 3 ? 2 : num % 4;
            result += n + part;
        }

        Console.WriteLine(result);
    }

    public static void D()
    {
        var arr = Enumerable.Range(0, 8).Select(_ => Console.ReadLine()![..8].ToArray()).ToList();

        var bees = new[] { (-1, 1), (1, 1), (1, -1), (-1, -1) };
        var rees = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

        void check((int, int)[] directions, char sym, int i, int j)
        {
            foreach (var (di, dj) in directions)
            {
                var ii = i;
                var jj = j;
                while (ii + di is >= 0 and < 8 && jj + dj is >= 0 and < 8 && arr[ii + di][jj + dj] is not ('B' or 'R'))
                {
                    ii += di;
                    jj += dj;
                    arr[ii][jj] = char.ToLower(sym);
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (arr[i][j] is 'B')
                {
                    check(bees, 'B', i, j);
                }
                else if (arr[i][j] is 'R')
                {
                    check(rees, 'R', i, j);
                }
            }
        }


        // void print()
        // {
        //     for (int i = 0; i < 8; i++)
        //     {
        //         Console.WriteLine(new string(arr[i]));
        //     }
        // }
        //
        // Console.WriteLine();
        // print();

        var result = arr.SelectMany(a => a).Select(s => s is '*' ? 1 : 0).Sum();
        Console.WriteLine(result);
    }


    public static void E()
    {
        var aArr = Console.ReadLine()!.Trim().Split().Select(long.Parse).ToList();
        var n = aArr[0];
        var k = aArr[1];
        var d = aArr[2];

        long result = -1;
        for (long i = 0; i <= 9; i++)
        {
            if ((n * 10 + i) % k == 0)
            {
                result = i;
                break;
            }
        }

        if (result == -1)
        {
            Console.WriteLine(-1);
        }
        else
        {
            var num = (n * 10 + result).ToString();

            Console.WriteLine(num + new string('0', (int)d - 1));
        }
    }

    public static void F()
    {
        Console.ReadLine();
        var arr = Console.ReadLine()!.Trim().Split().Select(long.Parse).Select(n => Math.Abs(n % 2) == 1).ToList();
        var compressed = new List<(bool val, int cnt)> { (arr[0], 0) };

        for (int i = 0; i < arr.Count; i++)
        {
            if (compressed[^1].val == arr[i])
            {
                compressed[^1] = (arr[i], compressed[^1].cnt + 1);
            }
            else
            {
                compressed.Add((arr[i], 1));
            }
        }

        if (compressed.Count == 1)
        {
            Console.WriteLine(new string('*', arr.Count - 1));
            return;
        }

        var betweenSigns = Enumerable.Repeat('&', compressed.Count - 1).ToList();

        if (compressed[^1].val == true)
        {
            betweenSigns[^1] = '^';
        }
        else
        {
            betweenSigns[^1] = '^';
            if (betweenSigns.Count > 1)
            {
                betweenSigns[^2] = '^';
            }
        }

        var result = new List<string>();
        for (int i = 0; i < compressed.Count - 1; i++)
        {
            result.Add(new string('x', compressed[i].cnt - 1));
            result.Add(betweenSigns[i] == '&' ? "x" : "+");
        }

        result.Add(new string('x', compressed[^1].cnt - 1));
        Console.WriteLine(string.Join("", result));
    }

    public static void G()
    {
        var myUnits = int.Parse(Console.ReadLine()!);
        var casarm = int.Parse(Console.ReadLine()!);
        var enemyTurnUnits = int.Parse(Console.ReadLine()!);

        var stage = 1;
        casarm -= myUnits;
        if (casarm <= 0)
        {
            Console.WriteLine(1);
            return;
        }

        var enemyUnits = enemyTurnUnits;

        while (myUnits > 0 && (casarm > 0 || enemyUnits > 0))
        {
            stage += 1;
            var startCasarm = casarm;
            //1
            if (enemyUnits > myUnits)
            {
                if (casarm > 0)
                {
                    casarm -= myUnits;
                    if (casarm < 0)
                    {
                        enemyUnits -= Math.Abs(casarm);
                    }
                }
                else
                {
                    enemyUnits -= myUnits;
                }
            }
            else
            {
                if (casarm > 0 && casarm <= myUnits && myUnits >= 2 * (enemyUnits - Math.Abs(casarm - myUnits)))
                {
                    casarm -= myUnits;
                    enemyUnits -= Math.Abs(casarm);
                }
                else
                {
                    enemyUnits -= myUnits;
                    casarm -= Math.Abs(enemyUnits);
                }
            }

            if (casarm > 0 && startCasarm == casarm)
            {
                Console.WriteLine(-1);
                return;
            }

            casarm = Math.Max(0, casarm);
            enemyUnits = Math.Max(0, enemyUnits);
            //2
            myUnits -= enemyUnits;
            //3
            if (casarm > 0)
            {
                enemyUnits += enemyTurnUnits;
            }
        }

        if (myUnits > 0)
        {
            Console.WriteLine(stage);
        }
        else
        {
            Console.WriteLine(-1);
        }
    }

    public static void G2()
    {
        var myUnitsStart = int.Parse(Console.ReadLine()!);
        var casarmStart = int.Parse(Console.ReadLine()!);
        var enemyTurnUnits = int.Parse(Console.ReadLine()!);

        var startStage = 1;
        casarmStart -= myUnitsStart;
        if (casarmStart <= 0)
        {
            Console.WriteLine(1);
            return;
        }

        var bestResult = -1;

        void calculateResult(int sRound)
        {
            var stage = startStage;
            var myUnits = myUnitsStart;
            var casarm = casarmStart;
            var enemyUnits = enemyTurnUnits;

            while (myUnits > 0 && (casarm > 0 || enemyUnits > 0))
            {
                stage += 1;
                var startCasarm = casarm;
                //1
                if (stage >= sRound)
                {
                    casarm -= myUnits;
                    if (casarm < 0)
                    {
                        enemyUnits -= Math.Abs(casarm);
                    }
                }
                else
                {
                    enemyUnits -= myUnits;
                    if (enemyUnits < 0)
                    {
                        casarm -= Math.Abs(enemyUnits);
                    }
                }

                if (casarm > 0 && startCasarm == casarm)
                {
                    return;
                }

                casarm = Math.Max(0, casarm);
                enemyUnits = Math.Max(0, enemyUnits);
                //2
                myUnits -= enemyUnits;
                //3
                if (casarm > 0)
                {
                    enemyUnits += enemyTurnUnits;
                }
            }

            if (myUnits > 0 && (bestResult == -1 || stage < bestResult))
            {
                bestResult = stage;
            }
        }

        for (int cRounds = 2; cRounds <= 5000; cRounds++)
        {
            calculateResult(cRounds);
        }

        Console.WriteLine(bestResult);
    }

    public static void J()
    {
        using var reader = new StreamReader("input.txt");
        var inArr = reader.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var w = inArr[0];
        var h = inArr[1];
        var c = inArr[2];
        var paragraphs = new List<string>() { "" };
        while (reader.ReadLine() is var line && line is not null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                paragraphs.Add("");
            }
            else
            {
                paragraphs[^1] += line;
            }
        }


        var parsedParagraphs = paragraphs.Select(p =>
                Regex.Matches(p, @"[\w.,:;!?-]+|\([\w =-]+\)| ").Select(m => m.ToString()).ToList())
            .ToList();

        var cursor = new Vector(0, 0);
        var images = new List<Image>();

        foreach (var parsedParagraph in parsedParagraphs)
        {
            foreach (var word in parsedParagraph)
            {
                if (word.StartsWith("(image"))
                {
                    
                }
                else
                {
                    
                }
            }
        }
    }


    public static void Test()
    {
        var s = "";
        Console.WriteLine(string.IsNullOrWhiteSpace(s));
    }

    private record struct Vector(int ii, int jj);

    record AlmostWord(int ii, int jj, int height, int width);

    record DefWord(int ii, int jj, int height, int width, string word) : AlmostWord(ii, jj, height, width);

    record Image(int ii, int jj, int height, int width) : AlmostWord(ii, jj, height, width);

    record EmbeddedImage(int ii, int jj, int height, int width) : Image(ii, jj, height, width);

    record SurroundedImage(int ii, int jj, int height, int width) : Image(ii, jj, height, width);

    record FloatingImage(int ii, int jj, int height, int width, int dx, int dy) : Image(ii, jj, height, width);
}