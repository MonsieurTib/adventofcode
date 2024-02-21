var lines = File.ReadAllLines("input.txt");

Part1();

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

