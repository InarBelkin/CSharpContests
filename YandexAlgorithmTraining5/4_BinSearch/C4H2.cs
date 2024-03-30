using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex5._4;

public static class C4H2
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

        var sortedParties = parties.OrderByDescending(p => p.People)
            .ThenByDescending(p => p.Bribe == -1 ? long.MaxValue : p.Bribe)
            .ToArray();
        var sumsOfParties = getSumsOfSortedParties(sortedParties);

        long? minPayment = null;
        Party? minPartyToInvest = null;
        long? minCntOfPeopleWhoWillChoseWinner = null;
        int indexOfParty = 0;
        for (int si = 0; si < sortedParties.Length; si++)
        {
            var partyToInvest = sortedParties[si];
            if (partyToInvest.Bribe == -1 ||
                (si != sortedParties.Length - 1 && sortedParties[si + 1].People == partyToInvest.People)) continue;

            var cntOfPeopleWhoWillChoseWinner = lBinSearch(partyToInvest.People, parties.Sum(p => p.People), m =>
            {
                long cutted = 0;
                for (int i = 0; i < si; i++)
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
            var curPayment = cntOfPeopleWhoWillChoseWinner - partyToInvest.People + partyToInvest.Bribe;
            if (minPayment is null || curPayment < minPayment)
            {
                minPayment = curPayment;
                minPartyToInvest = partyToInvest;
                minCntOfPeopleWhoWillChoseWinner = cntOfPeopleWhoWillChoseWinner;
                indexOfParty = si;
            }
        }

        var answer = GetAnswer(minPartyToInvest!, indexOfParty, minCntOfPeopleWhoWillChoseWinner!.Value, sortedParties);
        Console.WriteLine(minPayment!.Value);
        Console.WriteLine(minPartyToInvest.Id + 1);
        Console.WriteLine(string.Join(" ", answer.Select(p => p.People)));
    }

    private static List<PartyCopy> GetAnswer(Party partyToInvest, int indexOfParty, long cntOfPeopleWhoWillChoseWinner,
        Party[] sortedParties)
    {
        long cutted = 0;
        for (int i = 0; i < indexOfParty; i++)
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

        var copies = sortedParties.Where(p => p.Id != partyToInvest.Id)
            .Select(p => new PartyCopy() { Id = p.Id, People = p.People }).ToList();
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


    private static long[] getSumsOfSortedParties(Party[] parties)
    {
        var sums = new long[parties.Length];
        sums[0] = parties[0].People;
        for (int i = 1; i < parties.Length; i++)
        {
            sums[i] = sums[i - 1] + parties[i].People;
        }

        return sums;
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