using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Yandex5._3;

public static class C3J
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

        //count -> partId[]
        // var frequencies = new SortedDictionary<int, SortedSet<int>>()
        //     { { 1, new SortedSet<int>(Enumerable.Range(0, k)) } };
        var partFrequencies = new int[k];
        Array.Fill(partFrequencies, 1);

        var devicesThatContainPart = Enumerable.Range(0, k).Select(_ => new List<Device> { devices[0] }).ToArray();
        var currentTimespot = 1;
        while (devices.Where(d => d.TimeFinish is null).ToList() is var unfinished && unfinished.Count > 0)
        {
            //partId->sendtoThisDevice
            var requests = new Dictionary<int, Request>();
            foreach (var device in unfinished)
            {
                var partToRequest = device.MissingParts.MinBy(p => (partFrequencies[p], p));

                if (!requests.ContainsKey(partToRequest))
                {
                    requests[partToRequest] = new Request();
                }

                requests[partToRequest].Targets.Add(device);
            }

            var resolvedRequests = new List<ResolvedRequest>();
            foreach (var (partId, request) in requests)
            {
                var source = devicesThatContainPart[partId].MinBy(d => (d.HaveParts.Count, d.Id))!;
                var target = request.Targets
                    // .OrderByDescending(d => source.CountOfRecievedPartsFrom[d.Id]).ThenBy(d => d.HaveParts.Count)
                    // .ThenBy(d => d.Id).First();
                    .MinBy(d => (0 - source.CountOfRecievedPartsFrom[d.Id], d.HaveParts.Count, d.Id))!;
                resolvedRequests.Add(new ResolvedRequest(partId, source, target));
            }

            foreach (var (partId, source, target) in resolvedRequests)
            {
                PassPart(partId, source, target);
            }

            currentTimespot++;
        }

        Console.WriteLine(string.Join(' ', devices.Skip(1).Select(d => d.TimeFinish!.Value)));

        return;

        void PassPart(int partId, Device source, Device target)
        {
            target.RecievePart(partId, source, currentTimespot);
            partFrequencies[partId]++;
            devicesThatContainPart[partId].Add(target);
        }
    }

    private class Request
    {
        public readonly List<Device> Targets = new();

        public Device? SelectedSource = null;

        public Request()
        {
        }
    }

    private record ResolvedRequest(int partId, Device source, Device target);
}

public class Device
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

    public void RecievePart(int partId, Device source, int currentTimeSpot)
    {
        var success = MissingParts.Remove(partId);
        if (!success)
            throw new ArgumentException($"I don't miss part id = {partId}");
        HaveParts.Add(partId);
        CountOfRecievedPartsFrom[source.Id]++;
        if (MissingParts.Count == 0)
        {
            TimeFinish = currentTimeSpot;
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