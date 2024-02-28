using System.Diagnostics;
using System.Text;

var lines = File.ReadAllLines("input.txt");

var watcher = Stopwatch.StartNew();
Part2();
Console.WriteLine(watcher.Elapsed.TotalMilliseconds);

void Part1()
{
    var parts = lines.Select(line => line.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1));
    var races = parts.First().Zip(parts.Last())
        .Select( race => new
        {
            Time = Convert.ToInt32(race.First),
            Distance = Convert.ToInt32(race.Second)
        }).ToList();
    var winWays = new List<int>(races.Count);
    foreach (var race in races)
    {
        var pressTime = 1;
        var raceWinWays = 0;
        while (pressTime < race.Time)
        {
            var distance = (race.Time - pressTime) * pressTime;
            if (distance > race.Distance)
            {
                raceWinWays++;
            }
            pressTime++;
        }
        winWays.Add(raceWinWays);
    }
    
    Console.WriteLine(winWays.Aggregate(1, (a, b) => a *b));
}

void Part2()
{
    var parts = lines.Select(line => line.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1));
   
    var time = Convert.ToInt64(parts.First().Aggregate(new StringBuilder(), (builder, s) => builder.Append(s)).ToString());
    var distance =  Convert.ToInt64(parts.Last().Aggregate(new StringBuilder(), (builder, s) => builder.Append(s)).ToString());
  
    var pressTime = 1;
    var raceWinWays = 0;
    while (pressTime < time)
    {
        var dst = (time - pressTime) * pressTime;
        if (dst > distance)
        {
            raceWinWays++;
        }
        pressTime++;
    }
   
    Console.WriteLine(raceWinWays);
}

