namespace AdventOfCode23.Day2;

public class Day2_Part2
{
    public static void Run()
    {
        StreamReader sr = new StreamReader("F:\\Coding\\adventOfCode\\AdventOfCode23\\AdventOfCode23.Day2\\input1.txt");

        int lineResult;
        int result = 0;
        int lineNr = 0;
        while (!sr.EndOfStream)
        {
            lineResult = GetLineResult(sr.ReadLine());
            Console.WriteLine($"game {++lineNr,2}: {lineResult}");
            result += lineResult;           
        }
        Console.WriteLine();
        Console.WriteLine($"sum of powers: {result}");

    }



    static List<string> substringBuffer = new List<string>();
    static string[]? game;

    static int numberIndex;
    static int colorIndex;

    private static int GetLineResult(string line)
    {
        int maxRed = 0;
        int maxGreen = 0;
        int maxBlue = 0;

        substringBuffer.Clear();
        substringBuffer = line.Split(": ").ToList();
        int id = int.Parse(substringBuffer.First().Split(' ').Last());

        game = substringBuffer.Last().Replace(";", "").Replace(",", "").Split(' ');

        numberIndex = 0; colorIndex = 1;
        int comp;
        for (int i = 0; i < game.Length / 2; i++)
        {
            comp = int.Parse(game[numberIndex]);
            switch (game[colorIndex])
            {
                case "red":
                    if(comp > maxRed) 
                        maxRed = comp;
                    break;
                case "green":
                    if (comp > maxGreen) 
                        maxGreen = comp;                    
                    break;
                case "blue":
                    if (comp > maxBlue) 
                        maxBlue = comp;
                    break;
            }
            colorIndex += 2;
            numberIndex += 2;
        }
        return maxBlue * maxGreen * maxRed;
    }
}
