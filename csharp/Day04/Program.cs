var lines = File.ReadAllLines("input.txt");

Part2();

void Part1()
{
    var result = lines.Select(line => line.Split([':', '|']))
        .Select(cardPart => new
        {
            It = cardPart[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Intersect(cardPart[2].Split(" ", StringSplitOptions.RemoveEmptyEntries))
        })
        .Where(xp => xp.It.Any())
        .Select(s => new
        {
            Points = (s.It.Aggregate(1, (i, s1) => i * 2)/2)
        }).Sum( s => s.Points);
    Console.WriteLine(result);
}

void Part2()
{
    var result = lines.Select(line => line.Split([':', '|']))
        .Select((cardPart, index) => new
        {
            Index = index,
            Count = cardPart[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Intersect(cardPart[2].Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .Count()
        }).Aggregate( Enumerable.Range(0, lines.Length).ToDictionary( l => l, s => 0) , (dic, card) =>
        {
            dic[card.Index] += 1;
            for (var i = 1; i <= card.Count; i++)
            {
                dic[card.Index + i] += dic[card.Index];
            }
            
            return dic;
        }).Sum( s => s.Value);
        
    Console.WriteLine(result);
}