using System.Diagnostics;

var lines = File.ReadAllLines("input.txt");
var sort = "ABCDEFGHIJKLMNOP".ToCharArray();
var watcher = Stopwatch.StartNew();
Part2();
Console.WriteLine(watcher.Elapsed.TotalMilliseconds);
//250053758
void Part1()
{
    var strengths = "AKQJT98765432".ToCharArray();
    var hands = lines.Select(line => line.Split(" "))
        .Select(parts => new
        {
            Hand = parts.First(),
            Bid = Convert.ToInt32(parts.Last()),
            HandType = parts.First().ToHandType(),
            StrengthSort = string.Join("", parts
                .First()
                .ToCharArray()
                .Aggregate("", ((s, c) =>
                {
                    var index = strengths.Select((s, idx) => (s, idx)).First(_ => _.s.Equals(c)).idx;
                    s += sort.Select((s, idx) => (s, idx)).First(_ => _.idx.Equals(index)).s;
                    return s;
                } ))
                .ToList())
        })
        .OrderBy(o => o.HandType)
        .ThenBy(o => o.StrengthSort);
    var sum = 0;
    var inc = hands.Count();
    foreach (var hand in hands)
    {
        Console.WriteLine(hand.Hand);
        sum += hand.Bid * inc;
        inc--;
    }
    Console.WriteLine( sum);
}
//GOOD 251824095
void Part2()
{
    var strengths = "AKQT98765432J".ToCharArray();
    var hands = lines.Select(line => line.Split(" "))
        .Select(parts => new
        {
            Joker = parts.First().ToJoker(strengths) ,
            Hand = parts.First(),
            Bid = Convert.ToInt32(parts.Last())
        })
        .Select(parts => new
        {
            parts.Hand,
            parts.Bid,
            parts.Joker,
            HandType =  parts.Joker
                .ToHandType(),
            StrengthSort = string.Join("", parts.Hand
                .Aggregate("", (s, c) =>
                {
                    var index = strengths.Select((s, idx) => (s, idx)).First(_ => _.s.Equals(c)).idx;
                    s += sort.Select((s, idx) => (s, idx)).First(_ => _.idx.Equals(index)).s;
                    return s;
                })
                .ToList())
        })
        .OrderBy(o => o.HandType)
        .ThenBy(o => o.StrengthSort);
    var sum = 0;
    var inc = hands.Count();
    var current = HandType.Nothing;
    foreach (var hand in hands)
    {
        if (current != hand.HandType)
        {
            current = hand.HandType;
            Console.WriteLine($"--------------{current}-------------- ");
        }
        Console.WriteLine("\t" + hand.Hand + " ** " + hand.Joker + " ** " + hand.StrengthSort);
        sum += hand.Bid * inc;
        inc--;
    }
    Console.WriteLine( sum);
}

internal enum HandType
{
    FiveKind,
    FourKind,
    FullHouse,
    ThreeKind,
    TwoPair,
    OnePair,
    HighCard,
    Nothing
};

static class HandExtensions
{
    public static HandType ToHandType(this string hand)
    {
        var split = hand.GroupBy(c => c)
            .Select(grp => new
            {
                C = grp.Key,
                Count = grp.Count()
            }).OrderByDescending(s => s.Count).ToList();
        
        switch (split.First().Count)
        {
            case 5:
                return HandType.FiveKind;
            case 4:
                return HandType.FourKind;
            case 3 when split.Last().Count == 2:
                return HandType.FullHouse;
            case 3:
                return HandType.ThreeKind;
        }

        if (split.Count(s => s.Count == 2) == 2)
        {
            return HandType.TwoPair;
        }
        
        if (split.Count(s => s.Count == 2) == 1)
        {
            return HandType.OnePair;
        }
        
        return split.All(s => s.Count == 1) ? HandType.HighCard : HandType.Nothing;
    }

    public static string ToJoker(this string hand, char[] strengths)
    {
        if (hand == "JJJJJ")
        {
            return "AAAAA";
        }
       
        var split = hand.Replace("J", string.Empty).GroupBy(c => c)
            .Select(grp => new
            {
                C = grp.Key,
                Count = grp.Count()
            }).OrderByDescending(s => s.Count).ToList();
       
        switch (split.First().Count)
        {
            case >= 3:
                return hand.Replace('J', hand.ToCharArray()
                    .GroupBy(c => c)
                    .Select(grp => new { C = grp.Key, Count = grp.Count() })
                    .OrderByDescending(o => o.Count).First().C);
            case 2 when split.Count == 1 || split[1].Count is not 2:
                return hand.Replace('J', split[0].C);
            case 2:
            {
                foreach (var c in strengths)
                {
                    if (hand.Contains(c))
                    {
                        return hand.Replace('J', c);
                    }
                }
                return hand.Replace('J', split[0].C);
            }
        }

        foreach (var c in strengths)
        {
            if (hand.Contains(c))
            {
                hand = hand.Replace('J', c);
            }
        }

        return hand;
    }
}