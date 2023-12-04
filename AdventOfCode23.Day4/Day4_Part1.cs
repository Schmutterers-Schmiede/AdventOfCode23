using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day4
{
    public static class Day4_Part1
    {
        public static void Run()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day4/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());
            string line;
            int result = 0;
            int lineNr = 0;
            while (!sr.EndOfStream)
            {
                lineNr++;
                line = sr.ReadLine();
                int lineResult = LineResult(line);
                result += lineResult;
                Console.WriteLine($"Card {lineNr}: {lineResult} points");
            }
            Console.WriteLine();
            Console.WriteLine($"sum of points: {result}");
        }

        private static int LineResult(string line)
        {            
            var substringbuffer = Regex.Replace(Regex.Replace(line,"Card *[0-9]*: *", ""), @"\s+", " ").Split(" | ").ToList();
            List<int> cardNumbers = new List<int>();
            List<int> winningNumbers = new List<int>();
            foreach (string number in substringbuffer.First().Split(" "))
            {
                cardNumbers.Add(int.Parse(number));
            }
            foreach (string number in substringbuffer.Last().Split(" "))
            {
                winningNumbers.Add(int.Parse(number));
            }

            int points = 0;
            foreach (int cardNumber in cardNumbers)
            {
                if (winningNumbers.Contains(cardNumber))
                {
                    points += points == 0 ? 1 : points;
                }
            }
            return points;
        }
    }
}
