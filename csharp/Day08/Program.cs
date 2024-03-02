using System.Diagnostics;

var lines = File.ReadAllLines("sample.txt");
var watcher = Stopwatch.StartNew();

Part2();
Console.WriteLine(watcher.Elapsed.TotalMilliseconds);

void Part1()
{
    var directions = lines.First().ToCharArray().Select(c => c == 'L' ? 0 : 1)
        .ToList();
    
    var network = lines.Skip(2).Select(line => line
        .Replace("(", string.Empty)
        .Replace(")", string.Empty)
        .Split(new[] { '=', ',' }, StringSplitOptions.TrimEntries))
        .ToDictionary( k => k.First(), v => v.Skip(1).ToList());
   
    var key = "AAA";
    var steps = 1;
    var directionStep = 0;
    while (true)
    {
        if (!network.TryGetValue(key, out var nodes))
        {
            continue;
        }

        key = nodes[directions[directionStep]];
        if (directionStep + 1 < directions.Count)
        {
            directionStep++;
        }
        else
        {
            directionStep = 0;
        }

        if (key.Equals("ZZZ"))
        {
            break;
        }

        steps++;
    }
    
    Console.WriteLine(steps);
}

void Part2()
{
    var directions = lines.First().ToCharArray();

    var network = lines.Skip(2).Select(line => line
            .Replace("(", string.Empty)
            .Replace(")", string.Empty)
            .Split(new[] { '=', ',' }, StringSplitOptions.TrimEntries))
        .ToDictionary( k => k.First(), v => v.Skip(1).ToList());

    var startingNodes = network.Keys.Where(k => k.EndsWith("A")).ToDictionary(s => s, s => s);
    var stepsNode = startingNodes
        .ToDictionary<KeyValuePair<string, string>, string, long>(n => n.Key, n => 0);

    foreach (var (k,_) in startingNodes)
    {
        var found = false;
        while (true)
        {
            foreach (var direction in directions)
            {
                stepsNode[k] += 1;
                var v = startingNodes[k];
                if (direction.Equals('L'))
                {
                    startingNodes[k] = network[v][0];
                }
                else
                {
                    startingNodes[k] = network[v][1];
                }

                if (!startingNodes[k].EndsWith("Z"))
                {
                    continue;
                }

                found = true;
                break;
            }

            if (found)
            {
                break;
            }
        }
        
    }
    /*
    foreach (var t in stepsNode)
    {
        Console.WriteLine(" ******* " + t.Key + " - " + t.Value);
    }*/
    var steps = LeastCommonMultiple(stepsNode.Select(x => x.Value));
    Console.WriteLine(steps);
}

long LeastCommonMultiple(IEnumerable<long> numbers)
{
    var temp = "";
    return numbers.Aggregate(1L, (current, number) =>
    {
        return current / GreatestCommonDivisor(current, number) * number;
    });

}

long GreatestCommonDivisor(long a, long b)
{
    while (b != 0)
    {
        a %= b;
        (a, b) = (b, a);
    }

    return a;
}