using System.Diagnostics;

var lines = File.ReadAllLines("input.txt");

var watcher = Stopwatch.StartNew();
Part2();
Console.WriteLine(watcher.Elapsed.TotalMilliseconds);

void Part1()
{
    var seeds = lines.First().Split(':').Last().Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(s => Convert.ToInt64(s));
    
    var maps = new Dictionary<string, List<Map>>();
    var current = "";
    foreach (var line in lines.Skip(1).Where( l => !string.IsNullOrEmpty(l)))
    {
        if (line.Contains("map"))
        {
            current = line.Split(' ').First().Split('-').Last();
            maps.Add(current, []);
        }
        else
        {
            var parts = line.Split(" ");
            maps[current].Add(new Map(Convert.ToInt64(parts[0]), Convert.ToInt64(parts[1]), Convert.ToInt64(parts[2])));
        }
    }

    var locationValue = long.MaxValue;
    
    foreach (var seed in seeds)
    {
        var lastSeed = seed;
        foreach (var map in maps)
        {
            foreach (var m in map.Value)
            {
                if (!m.TryGetSeedMap(lastSeed, out var value))
                {
                    continue;
                }

                lastSeed = value;
                break;
            }

            //Console.WriteLine($"\t{map.Key} : {lastSeed}");
            if (!map.Key.Equals("location"))
            {
                continue;
            }

            locationValue = Math.Min(locationValue, lastSeed);
            break;
        }
    }
    
    Console.WriteLine($"Part 1 {locationValue}");
}

void Part2()
{
    var seeds = lines.First().Split(':').Last().Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(s => Convert.ToInt64(s))
        .ToList();
    
    var maps = new Dictionary<string, List<Map>>();
    var current = "";
    foreach (var line in lines.Skip(1).Where( l => !string.IsNullOrEmpty(l)))
    {
        if (line.Contains("map"))
        {
            current = line.Split(' ').First().Split('-').Last();
            maps.Add(current, []);
        }
        else
        {
            var parts = line.Split(" ");
            maps[current].Add(new Map(Convert.ToInt64(parts[0]), Convert.ToInt64(parts[1]), Convert.ToInt64(parts[2])));
        }
    }
    var locationValue = long.MaxValue;

    for (var i = 0; i < seeds.Count; i += 2)
    {
        var start = seeds[i];
        var max = start + seeds[i + 1];
        Console.WriteLine( start) ;
        Parallel.For(start, max,  seed =>
        {
            var lastSeed = seed;
            foreach (var map in maps)
            {
                foreach (var m in map.Value)
                {
                    if (!m.TryGetSeedMap(lastSeed, out var value))
                    {
                        continue;
                    }

                    lastSeed = value;
                    break;
                }
                
                if (map.Key.Equals("location") && locationValue > lastSeed)
                {
                    Interlocked.Exchange(ref locationValue, lastSeed);
                    break;
                }
            }
        });
    }
    Console.WriteLine($"Part 2 {locationValue}");
}

internal readonly struct Map(long dest, long src, long range)
{
    private readonly long _max = src + range  ;
    private readonly long _delta = dest - src;
    public bool TryGetSeedMap(long seed, out long value)
    {
        value = 0;
        if (seed < src || seed > _max)
        {
            return false;
        }

        value = seed + _delta;
        return true;
    }
}