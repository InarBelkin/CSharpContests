namespace YandexAlgorithmTraining5._4_BinSearch;

public static class C4A
{
    public static void Solution()
    {
        var N = int.Parse(Console.ReadLine()!);
        var arr = Console.ReadLine()!.Trim().Split().Select(int.Parse).OrderBy(n => n).ToArray();

        var k = int.Parse(Console.ReadLine()!);
        var result = new List<int>();
        for (int i = 0; i < k; i++)
        {
            var lrArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
            var l = lrArr[0];
            var r = lrArr[1];

            var lIndex = lBinSearch(0, arr.Length - 1, m => arr[m] >= l);
            if (arr[lIndex] < l)
            {
                result.Add(0);
                continue;
            }

            var rIndex = rBinSearch(0, arr.Length - 1, m => arr[m] <= r);
            if (arr[rIndex] > r)
            {
                result.Add(0);
                continue;
            }

            result.Add(rIndex - lIndex + 1);
        }

        Console.WriteLine(String.Join(" ", result));
    }

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