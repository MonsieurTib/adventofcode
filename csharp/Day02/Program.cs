var lines = File.ReadAllLines("input.txt");

Part1();
Part2();

void Part1()
{
    var result = lines.Select(line => line.Split([':', ';']))
        .Select(line => new
        {
            Id = line[0].Split(' ').Last(),
            Set = line.Skip(1).Select(p => p.Split(',').Select(xp => new
                {
                    Value = Convert.ToInt32(xp.Split(' ').Skip(1).First()),
                    Color = xp.Split(' ').Last()
                })).SelectMany(xp => xp)
                .GroupBy(grp => grp.Color)
                .Select(g => new
                {
                    Color = g.Key,
                    Max = g.Max(s => s.Value)
                })
                .ToDictionary( k => k.Color, t => t.Max)
        })
        .Where( game => (game.Set.TryGetValue("red", out var redValue) && redValue <= 12 )
            && (game.Set.TryGetValue("green", out var greenValue) && greenValue <= 13 )
            && (game.Set.TryGetValue("blue", out var blueValue) && blueValue <= 14 )
        ).Sum( c => Convert.ToInt32(c.Id));
       
       Console.WriteLine(result);
}

void Part2()
{
    var result = lines.Select(line => line.Split([':', ';']))
        .Select(line => new
        {
            Id = line[0].Split(' ').Last(),
            PowerSet = line.Skip(1).Select(p => p.Split(',').Select(xp => new
                {
                    Value = Convert.ToInt32(xp.Split(' ').Skip(1).First()),
                    Color = xp.Split(' ').Last()
                })).SelectMany(xp => xp)
                .GroupBy(grp => grp.Color)
                .Select(g => new
                {
                    Color = g.Key,
                    Max = g.Max(s => s.Value)
                })
                .ToDictionary(k => k.Color, t => t.Max)
                .Aggregate(1, (i, pair) => i * pair.Value )
        }).Sum( s => s.PowerSet);
       
    Console.WriteLine(result);
}