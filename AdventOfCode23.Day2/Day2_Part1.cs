using System.Text;

namespace AdventOfCode23.Day2;

public class Day2_Part1
{
    public static void Run()
    {
        StringBuilder path = new StringBuilder();
        path.Append(AppDomain.CurrentDomain.BaseDirectory);
        path.Append("../../../../AdventOfCode23.Day2/input1.txt");
        StreamReader sr = new StreamReader(path.ToString());
                     
            int lineResult;
            int result = 0;
            int lineNr = 0;
            while (!sr.EndOfStream)
            {
                
                lineResult = GetLineResult(sr.ReadLine());
                Console.Write($"game {++lineNr}: ");
                if (lineResult != -1)
                {
                    Console.WriteLine("Yes");
                    result += lineResult;
                }
                else
                    Console.WriteLine("No");

            }
            Console.WriteLine();
            Console.WriteLine($"Id sum of possible Games: {result}");

    }


    
    static List<string> substringBuffer = new List<string>();
    static string[]? game;

    static int numberIndex;
    static int colorIndex;

    private static int GetLineResult(string line)
    {        
        substringBuffer.Clear();
        substringBuffer = line.Split(": ").ToList();
        int id = int.Parse(substringBuffer.First().Split(' ').Last());

        game = substringBuffer.Last().Replace(";","").Replace(",","").Split(' ');

        numberIndex = 0; colorIndex = 1;
        int comp;
        for (int i = 0; i < game.Length/2; i++)
        {
            comp = int.Parse(game[numberIndex]);
            switch (game[colorIndex])
            {
                case "red": 
                    if(comp > 12)
                        return -1;
                    break;
                case "green":
                    if (comp > 13)
                        return -1;
                    break;
                case "blue":
                    if (int.Parse(game[numberIndex]) > 14)
                        return -1;
                    break;
            }
            colorIndex += 2;
            numberIndex += 2;
        }
        return id;
    }
}
