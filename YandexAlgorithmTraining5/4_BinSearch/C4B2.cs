using System;

namespace Yandex5._4;

public static class C4B2
{
    public static void Solution()
    {
        var n = long.Parse(Console.ReadLine()!);
        long value = -1;
        long prevAddedValue = 0;
        for (long i = 0;; i++)
        {
            var curAddedValue = prevAddedValue + i + 2;
            var curValue = value + curAddedValue;

            if (curValue > n)
            {
                Console.WriteLine(i);
                return;
            }

            prevAddedValue = curAddedValue;
            value = curValue;
        }
    }
}