var lines = File.ReadAllLines("input.txt");

Part1();
Part2();

void Part1()
{
    var notSymbol = "0123456789.".ToCharArray();
    var symbols = lines.Select((line, index) => new
        {
            s = line.ToCharArray().Select((x, i) => (c: x, X: i, Y: index))
                .Where(_ => !notSymbol.Contains(_.c))
        })
        .SelectMany(m => m.s)
        .Select(m => new
        {
            m.X, m.Y
        }).GroupBy( grp => grp.Y).
        ToDictionary( g => g.Key, s => s
            .Select( _ => _.X)
            .ToHashSet());

    var lineIndex = 0;
    var sum = 0;
    foreach (var line in lines)
    {
        var chars = line.ToCharArray();
        var position = 0;
        List<(int lineIndex, int charIndex, char c)> current = [];
        
        while (position < chars.Length)
        {
             if (char.IsDigit(chars[position]))
             {
                 current.Add((lineIndex,position,chars[position]));

                 if (position == chars.Length - 1)
                 {
                     var hasAdj = FindAdj(symbols, current);
                     if (hasAdj && current.Count != 0)
                     {
                         sum += Convert.ToInt32(string.Join("", current.Select(d => d.c)));
                     }
                 }
             }
             else
             {
                 var hasAdj = FindAdj(symbols, current);
                 if (hasAdj && current.Count != 0)
                 {
                     sum += Convert.ToInt32(string.Join("", current.Select(d => d.c)));
                 }
                 current = [];
             }

             position++;
        }

        lineIndex++;
    }
    Console.WriteLine(sum);
}

void Part2()
{
    var gears = lines.Select((line, index) => new
        {
            s = line.ToCharArray().Select((x, i) => (c: x, X: i, Y: index))
                .Where(_ => _.c.Equals('*'))
        })
        .SelectMany(m => m.s)
        .Select(m => new
        {
            m.X, m.Y
        }).GroupBy( grp => grp.Y).
        ToDictionary( g => g.Key, s => s
            .Select( _ => _.X)
            .ToHashSet());

    var map = new Dictionary<int, Dictionary<int, (string hash, int number)>>();
    var lineIndex = 0;
    foreach (var line in lines)
    {
        var chars = line.ToCharArray();
        var position = 0;

        var digits = new List<(int index, char c)>();
        
        while (position < chars.Length)
        {
            var currentChar = chars[position];
            if (char.IsDigit(currentChar))
            {
                digits.Add((position,currentChar));
                if (position == chars.Length - 1)
                {
                    TryAddToMap(map, lineIndex, digits);
                }
            }
            else
            {
                if (digits.Count != 0)
                {
                    TryAddToMap(map, lineIndex, digits);
                    digits = [];
                }
            }

            position++;
        }

        lineIndex++;
    }

    var sum = 0;
    foreach (var gear in gears)
    {
        foreach (var gearRow in gear.Value)
        {
            var gearMap = new Dictionary<string, int>();
            GearAdj(map, gearMap, gearRow, gear.Key - 1);
            GearAdj(map, gearMap, gearRow, gear.Key );
            GearAdj(map, gearMap, gearRow, gear.Key + 1);
            
            if (gearMap.Count == 2)
            {
                sum += gearMap.Values.Aggregate(1, (a, b) => a * b);
            }
        }
    }
    Console.WriteLine(sum);
}

void TryAddToMap(IDictionary<int, Dictionary<int, (string hash, int number)>> map, int lineIndex, List<(int index, char c)> digits)
{
    if(!map.TryGetValue(lineIndex, out var value))
    {
        value = new Dictionary<int, (string hash, int number)>();
        map.Add(lineIndex, value);
    }
    foreach (var (index,c) in digits)
    {
        var hash = string.Join(lineIndex.ToString(), digits.Select(d => d.index));
        value.Add(index, ( hash, Convert.ToInt32(string.Join("",digits.Select(d => d.c)))));
    }
}

void GearAdj(IDictionary<int, Dictionary<int, (string hash, int number)>> map, Dictionary<string, int> gearMap, int gearRow, int lineIndex)
{
    if (!map.TryGetValue(lineIndex, out var previousRowIndexes))
    {
        return;
    }

    if(previousRowIndexes.TryGetValue(gearRow - 1, out var a))
    {
        gearMap.TryAdd(a.hash, a.number);
    }
    if(previousRowIndexes.TryGetValue(gearRow , out var b))
    {
        gearMap.TryAdd(b.hash, b.number);
    }
    if(previousRowIndexes.TryGetValue(gearRow +1 , out var c))
    {
        gearMap.TryAdd(c.hash, c.number);
    }
}

bool FindAdj(IReadOnlyDictionary<int, HashSet<int>> symbols, List<(int lineIndex, int charIndex, char c)> current)
{
    var hasAdj = false;
    foreach (var (lIndex,charIndex, _) in current)
    {
        if (!FindCharAdj(symbols, lIndex - 1, charIndex)
            && !FindCharAdj(symbols, lIndex, charIndex)
            && !FindCharAdj(symbols, lIndex + 1, charIndex))
        {
            continue;
        }

        hasAdj = true;
        break;
    }

    return hasAdj;
}

bool FindCharAdj(IReadOnlyDictionary<int, HashSet<int>> symbols, int lineIndex, int charIndex)
{
    if (!symbols.TryGetValue(lineIndex, out var previousLine))
    {
        return false;
    }

    return previousLine.Contains(charIndex) 
           || previousLine.Contains(charIndex + 1) 
           || previousLine.Contains(charIndex - 1);
}

