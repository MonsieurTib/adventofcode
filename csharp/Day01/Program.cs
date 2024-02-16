var lines = File.ReadAllLines("input.txt");

Part1();
Part2();

void Part1()
{
    var sum = 0;
    var digit = "123456456789".AsSpan();
    
    foreach (var line in lines)
    {
        var offset = 0;
        var input = line.AsSpan();
        var result = new List<char>();
        while (offset < input.Length)
        {
            var c = input[offset];
            var digitIndex = digit.IndexOf(c);
            if (digitIndex > -1)
            {
                result.Add(digit[digitIndex]);
                offset += 1;
                continue;
            }
            offset += 1;
        }
        sum += Convert.ToInt32(string.Concat(result.First(),result.Last()));
    }
    Console.WriteLine($"part1 : {sum}");
}

void Part2()
{
    var sum = 0;
    var digitsMap = new Dictionary<string, string>()
    {
        {"1","1"},
        {"2","2"},
        {"3","3"},
        {"4","4"},
        {"5","5"},
        {"6","6"},
        {"7","7"},
        {"8","8"},
        {"9","9"},
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" }
    };
    
    foreach (var line in lines)
    {
        var indexes = new List<(int index, string Value)>();
        foreach (var (key,value) in digitsMap)
        {
            var offset = 0;
            while (true)
            {
                var index = line.IndexOf(key,offset);
                if(index == -1)
                {
                    break;
                }

                offset = index+1;
                indexes.Add((index, digitsMap[key]));
            }
        }

        var digits = indexes.OrderBy(xp => xp.index);
        sum += Convert.ToInt32(string.Concat(digits.First().Value, digits.Last().Value));
    }
    Console.WriteLine($"part2 : {sum}");
}




