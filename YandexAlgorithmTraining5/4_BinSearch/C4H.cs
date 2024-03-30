using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex5._4;

public static class C4H
{
    public static void Solution()
    {
        var n = long.Parse(Console.ReadLine()!);
        var parties = Enumerable.Range(0, (int)n).Select(i =>
        {
            var vpArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
            var people = vpArr[0];
            var bribe = vpArr[1];
            return new Party(i, people, bribe);
        }).ToList();

        var partyToInvest = parties.Where(p => p.Bribe != -1).MaxBy(p => p.People - p.Bribe)!;
        var sortedParties = parties
            .Where(p => p.Id != partyToInvest.Id)
            .OrderByDescending(p => p.People).ToList();
        var cntOfPeopleWhoWillChoseWinner = lBinSearch(partyToInvest.People, parties.Sum(p => p.People), m =>
        {
            long cutted = 0;
            for (int i = 0; i < sortedParties.Count; i++)
            {
                if (sortedParties[i].People >= m)
                {
                    cutted += sortedParties[i].People - m + 1;
                }
                else
                {
                    break;
                }
            }

            return (cutted <= m - partyToInvest.People);
        });

        var answer = GetAnswer(partyToInvest, cntOfPeopleWhoWillChoseWinner, sortedParties);
        Console.WriteLine(cntOfPeopleWhoWillChoseWinner - partyToInvest.People + partyToInvest.Bribe);
        Console.WriteLine(partyToInvest.Id + 1);
        Console.WriteLine(string.Join(" ", answer.Select(p => p.People)));
    }

    private static List<PartyCopy> GetAnswer(Party partyToInvest, long cntOfPeopleWhoWillChoseWinner,
        List<Party> sortedParties)
    {
        long cutted = 0;
        for (int i = 0; i < sortedParties.Count; i++)
        {
            if (sortedParties[i].People >= cntOfPeopleWhoWillChoseWinner)
            {
                cutted += sortedParties[i].People - cntOfPeopleWhoWillChoseWinner + 1;
            }
            else
            {
                break;
            }
        }

        var leftToDelete = cntOfPeopleWhoWillChoseWinner - partyToInvest.People - cutted;

        var copies = sortedParties.Select(p => new PartyCopy() { Id = p.Id, People = p.People }).ToList();
        foreach (var partyCopy in copies)
        {
            partyCopy.People = Math.Min(cntOfPeopleWhoWillChoseWinner - 1, partyCopy.People);
            partyCopy.People -= leftToDelete;
            if (partyCopy.People < 0)
            {
                leftToDelete = -partyCopy.People;
                partyCopy.People = 0;
            }
            else
            {
                leftToDelete = 0;
            }
        }

        copies.Add(new PartyCopy() { Id = partyToInvest.Id, People = cntOfPeopleWhoWillChoseWinner });
        copies.Sort((p1, p2) => p1.Id.CompareTo(p2.Id));
        return copies;
    }

    private record Party(long Id, long People, long Bribe);

    private class PartyCopy
    {
        public long Id;
        public long People;
    }

    private static long lBinSearch(long l, long r, Func<long, bool> check)
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
}