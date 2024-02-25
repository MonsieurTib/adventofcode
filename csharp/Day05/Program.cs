var lines = File.ReadAllLines("sample.txt");

Part1();

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
            Console.WriteLine(current);
        }
        else
        {
            var parts = line.Split(" ");
            maps[current].Add(new Map(Convert.ToInt64(parts[0]), Convert.ToInt64(parts[1]), Convert.ToInt64(parts[2])));
        }
    }
    Console.WriteLine("#1");
    var locationValues = new List<long>();
    
    foreach (var seed in seeds)
    {
        Console.WriteLine($"seed {seed}");
       
        long lastSeed = seed;
        foreach (var map in maps)
        {
            var mapMatch = false;
            foreach (var m in map.Value)
            {
                if (m.TryGetSeedMap(lastSeed, out var value))
                {
                    lastSeed = value;
                    Console.WriteLine($"\t{map.Key} : {value}");
                    mapMatch = true;
                    if (map.Key == "location")
                    {
                        locationValues.Add(value);
                    }
                    break;
                }
            }
    
            if (mapMatch == false)
            {
                Console.WriteLine($"\t{map.Key} : {lastSeed}");
                if (map.Key == "location")
                {
                    locationValues.Add(lastSeed);
                }
            }
        }
    }
    
    Console.WriteLine($"Part 1 {locationValues.Min()}");
}





internal class Map(long dest, long src, long range)
{
    private readonly IEnumerable<long> _seedValues = Toto(src, range);
    private readonly IEnumerable<long> _categoryValues =Toto(dest, range).ToList();

    public bool TryGetSeedMap(long seed, out long value)
    {
        value = 0;
        var found = false;
        var index = _seedValues.Select((value, index) => new { seed = value, index}).FirstOrDefault( value => value.seed == seed)?.index;
        if (!index.HasValue)
        {
            return found;
        }

        found = true;
        value = _categoryValues.ToArray()[index.Value];
        return found;
    }
    
    static IEnumerable<long> Toto(long start, long count)
    {
        for (var i = start; i < start + count; i++)
        {
            yield return i;
        }
    }

}

