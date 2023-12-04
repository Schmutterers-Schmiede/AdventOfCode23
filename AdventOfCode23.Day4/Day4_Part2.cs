using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode23.main
{
    public static class Day4_Part2
    {
        public static void Run()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day4/input2.txt");
            StreamReader sr = new StreamReader(path.ToString());
            Dictionary<int, int> cards = new Dictionary<int, int>();
            int i = 0;
            while(!sr.EndOfStream)
            {
                cards.Add(i, CountMatches(sr.ReadLine()));
            }

        }
        private static int CountMatches(string line)
        {
            
            var substringbuffer = Regex.Replace(Regex.Replace(line, "Card *[0-9]*: *", ""), @"\s+", " ").Split(" | ").ToList();
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
            int matchCount = 0;
            foreach (int number in cardNumbers)
            {
                if (winningNumbers.Contains(number)) matchCount++;
            }
            return matchCount;
        }
        
    }
}
