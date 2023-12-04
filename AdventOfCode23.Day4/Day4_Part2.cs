using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode23.main
{
    public static class Day4_Part2
    {
        private static Dictionary<int, int> cards = new Dictionary<int, int>();
        
        public static void Run()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day4/input2.txt");
            StreamReader sr = new StreamReader(path.ToString());
            
            int i = 0;
            while(!sr.EndOfStream)
            {
                cards.Add(i + 1, CountMatches(sr.ReadLine()));
                i++;
            }
            
            int copyCount = 0;
            foreach(var card in cards)
            {
                Console.WriteLine($"Processing card {card.Key,-3} with match count {card.Value,-2}");
                copyCount += GetCopyCountForCard(card);
            }
            
            Console.WriteLine();
            Console.WriteLine($"Original cards: {cards.Count()}");
            Console.WriteLine($"Copies: {copyCount}");
            Console.WriteLine($"Total: {cards.Count() + copyCount}");

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
        
        
        private static int GetCopyCountForCard(KeyValuePair<int, int> card)
        {  
            List<KeyValuePair<int, int>> copies = GetCopiesForCard(card);
            int result = copies.Count;
            foreach(var copy in copies)
            {
                result += GetCopyCountForCard(copy);
            }
            return result;
        }
        

        private static List<KeyValuePair<int, int>> GetCopiesForCard(KeyValuePair<int, int> card)
        {
            List<KeyValuePair<int, int>> result = new List<KeyValuePair<int, int>>();
            if (card.Value == 0) return result;
            
            for (int i = card.Key + 1; i <= card.Key + card.Value; i++)
            {
                result.Add(new KeyValuePair<int, int>(i, cards[i]));                
            }            
            return result;
        }        
    }
}
