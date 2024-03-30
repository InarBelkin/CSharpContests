using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Yandex5._3._3JFolder;

public static class C3J2
{
    public static void Solution()
    {
        var nkArr = Console.ReadLine()!.Trim().Split().Select(int.Parse).ToList();
        var n = nkArr[0];
        var k = nkArr[1];

        var devices = new[] { new Device(0, n, k, true) { TimeFinish = 0 } }
            .Concat(
                Enumerable.Range(1, n - 1).Select(i => new Device(i, n, k)))
            .ToList();
        var currentTimeSlot = 1;
        while (devices.Where(d => d.MissingParts.Count > 0).ToList() is var unfinished && unfinished.Count > 0)
        {
            var countsOfThisPart = Enumerable.Range(0, k)
                .Select(partId => devices.Select(d => d.HaveParts.Contains(partId) ? 1 : 0).Sum()).ToList();

            var requests = new List<Request>();
            foreach (var device in unfinished)
            {
                var partIdToRequest = device.MissingParts.MinBy(partId => (countsOfThisPart[partId], partId));
                requests.Add(new Request(partIdToRequest, device));
            }

            var requestsWithSources = new List<RequestWithFoundSource>();
            foreach (var request in requests)
            {
                var source = devices.Where(d => d.HaveParts.Contains(request.PartId))
                    .MinBy(d => (d.HaveParts.Count, d.Id))!;
                requestsWithSources.Add(new RequestWithFoundSource(request.PartId, source, request.Target));
            }

            var groupedRequests = requestsWithSources.GroupBy(r => r.Source)
                .ToDictionary(g => g.Key, g => g.ToList());

            var onlyResolvedRequests = new List<RequestWithFoundSource>();
            foreach (var (source, targetRequests) in groupedRequests)
            {
                var selectedTarget = targetRequests
                    .MinBy(r => (0 - source.CountOfRecievedPartsFrom[r.Target.Id], r.Target.HaveParts.Count,
                        r.Target.Id))!;
                onlyResolvedRequests.Add(selectedTarget);
            }

            foreach (var (partId, source, target) in onlyResolvedRequests)
            {
                target.RecievePart(partId, source, currentTimeSlot);
            }

            currentTimeSlot++;
        }

        Console.WriteLine(string.Join(' ', devices.Skip(1).Select(d => d.TimeFinish!.Value)));
    }

    private record Request(int PartId, Device Target);

    private record RequestWithFoundSource(int PartId, Device Source, Device Target);

    private class Device
    {
        public readonly int Id;
        public readonly HashSet<int> HaveParts = new HashSet<int>();
        public readonly HashSet<int> MissingParts = new HashSet<int>();
        public int? TimeFinish = null;
        public readonly int[] CountOfRecievedPartsFrom;

        public Device(int deviceId, int countOfDevices, int countOfParts, bool hasAllParts = false)
        {
            this.Id = deviceId;
            this.CountOfRecievedPartsFrom = new int[countOfDevices];
            if (hasAllParts)
            {
                HaveParts.UnionWith(Enumerable.Range(0, countOfParts));
            }
            else
            {
                MissingParts.UnionWith(Enumerable.Range(0, countOfParts));
            }
        }

        public void RecievePart(int partId, Device source, int currentTime)
        {
            var resultOfDeletion = MissingParts.Remove(partId);
            if (!resultOfDeletion)
            {
                throw new ArgumentException("i don't miss it");
            }

            HaveParts.Add(partId);
            CountOfRecievedPartsFrom[source.Id]++;
            if (MissingParts.Count == 0)
            {
                TimeFinish = currentTime;
            }
        }
    }
}

public static class DictExtensions
{
    public static KeyValuePair<TKey, TValue> GetMin<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        return dictionary.First();
    }

    public static KeyValuePair<TKey, TValue> GetMax<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        var field = typeof(SortedDictionary<TKey, TValue>).GetField("_set",
            BindingFlags.Instance | BindingFlags.NonPublic);
        var set = field.GetValue(dictionary) as SortedSet<KeyValuePair<TKey, TValue>>;
        return set!.Max;
        // dictionary.Max()
    }
}