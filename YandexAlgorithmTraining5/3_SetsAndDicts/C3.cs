using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Yandex5;

public static class C3
{
    public static void A()
    {
        var n = int.Parse(Console.ReadLine()!);
        var favorites = Enumerable.Range(0, n).Select(_ =>
        {
            Console.ReadLine();
            return Console.ReadLine()!.Trim().Split().ToImmutableHashSet();
        }).ToList();

        var result = favorites.Aggregate((a, b) => a.Intersect(b));
        Console.WriteLine(result.Count);
        Console.WriteLine(string.Join(' ', result.OrderBy(s => s)));
    }

    public static void A2()
    {
        var n = int.Parse(Console.ReadLine()!);
        Console.ReadLine();
        var result = new HashSet<string>(Console.ReadLine()!.Trim().Split());
        for (int i = 1; i < n; i++)
        {
            Console.ReadLine();
            result.IntersectWith(Console.ReadLine()!.Trim().Split().ToImmutableHashSet());
        }

        // var result = favorites.Aggregate((a, b) => a.Intersect(b));
        Console.WriteLine(result.Count);
        Console.WriteLine(string.Join(' ', result.OrderBy(s => s)));
    }

    public static void B()
    {
        var a = Console.ReadLine()!.Trim();
        var b = Console.ReadLine()!.Trim();
        if (a.Length != b.Length)
        {
            Console.WriteLine("NO");
            return;
        }

        Dictionary<char, int> GetCounts(string s)
        {
            var result = new Dictionary<char, int>();
            foreach (var c in s)
            {
                result.TryAdd(c, 0);

                result[c]++;
            }

            return result;
        }

        var aDict = GetCounts(a);
        var bDict = GetCounts(b);
        foreach (var (key, value) in aDict)
        {
            if (bDict.TryGetValue(key, out var secValue))
            {
                if (value != secValue)
                {
                    Console.WriteLine("NO");
                    return;
                }
            }
            else
            {
                Console.WriteLine("NO");
                return;
            }
        }

        Console.WriteLine("YES");
    }

    public static void C()
    {
        Console.ReadLine();
        var numbers = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();

        var counts = new Dictionary<int, int>();
        foreach (var number in numbers)
        {
            counts.TryAdd(number, 0);
            counts[number]++;
        }

        int? minDeleted = null;
        foreach (var num in numbers.OrderBy(num => num))
        {
            var countLeft = counts[num] + (counts.TryGetValue(num + 1, out var secondValue) ? secondValue : 0);
            var countDeleted = numbers.Count - countLeft;
            if (minDeleted is null || minDeleted!.Value > countDeleted)
            {
                minDeleted = countDeleted;
            }
        }

        Console.WriteLine(minDeleted!.Value);
    }

    public static void D()
    {
        var nkArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var n = nkArr[0];
        var k = nkArr[1];
        var numbers = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();

        var lastIndexesOfNumber = new Dictionary<int, int>();
        for (int i = 0; i < numbers.Count; i++)
        {
            if (lastIndexesOfNumber.TryGetValue(numbers[i], out var prevIndex) && i - prevIndex <= k)
            {
                Console.WriteLine("YES");
                return;
            }

            lastIndexesOfNumber[numbers[i]] = i;
        }

        Console.WriteLine("NO");
    }

    public static void E()
    {
        var groups = Enumerable.Range(0, 3).Select(_ =>
        {
            Console.ReadLine();
            return Console.ReadLine()!.Trim().Split().Select(int.Parse).ToHashSet();
        }).ToList();
        var allNumbers = groups.SelectMany(g => g).ToHashSet();
        var result = new List<int>();
        foreach (var num in allNumbers)
        {
            if (groups.Select(g => g.Contains(num) ? 1 : 0).Sum() >= 2)
            {
                result.Add(num);
            }
        }

        Console.WriteLine(string.Join(' ', result.OrderBy(n => n)));
    }

    public static void F()
    {
        var vocabulary = Console.ReadLine()!.Trim().Split().ToHashSet();

        var words = Console.ReadLine()!.Trim().Split().ToList();
        var result = new List<string>();
        foreach (var word in words)
        {
            var cut = Enumerable.Range(1, word.Length).Select(i => word[..i])
                .FirstOrDefault(w => vocabulary.Contains(w));
            result.Add(cut ?? word);
        }

        Console.WriteLine(string.Join(' ', result));
    }
    
}