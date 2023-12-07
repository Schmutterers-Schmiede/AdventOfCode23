using System.Runtime.ExceptionServices;
using System.Text;

namespace AdventOfCode23.Day7
{
    public class Day7_Part1
    {
        /*
         * hand calculation table
         * 1 occurence      -> +10
         * pair             -> +100
         * trio             -> +1000
         * quartet          -> +10000
         * five of a kind   -> +100000
         * 
         * all different    = 50
         * 1 pair           = 130
         * 2 pairs          = 210
         * 3 of a kind      = 1020
         * full house       = 1100
         * 4 of a kind      = 10010
         * 5 of a kind      = 100000
         */

        private static List<Hand> hands = new List<Hand>();
        public static void Run()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day7/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());
            Init(sr);
            SortHands();
            long winnings = TotalWinnings();
            Console.WriteLine($"total winnings: {winnings}");

        }

        private static long TotalWinnings()
        {
            long result = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                result += hands[i].Bid * (i + 1);
            }
            return result;
        }

        private static void SortHands()
        {
            hands.Sort(new TypeComparer());
            List<Hand> noType = new List<Hand>();
            List<Hand> onePair = new List<Hand>();
            List<Hand> twoPairs = new List<Hand>();
            List<Hand> threeOfAKind = new List<Hand>();
            List<Hand> fullHouse = new List<Hand>();
            List<Hand> fourOfAKind = new List<Hand>();
            List<Hand> fiveOfAKind = new List<Hand>();
            
            noType =        hands.Where(hand => hand.Type == 50).ToList();
            onePair =       hands.Where(hand => hand.Type == 130).ToList();
            twoPairs =      hands.Where(hand => hand.Type == 210).ToList();
            threeOfAKind =  hands.Where(hand => hand.Type == 1020).ToList();
            fullHouse =     hands.Where(hand => hand.Type == 1100).ToList();
            fourOfAKind =   hands.Where(hand => hand.Type == 10010).ToList();
            fiveOfAKind =   hands.Where(hand => hand.Type == 100000).ToList();

            noType.Sort(new CardComparer());
            onePair.Sort(new CardComparer());
            twoPairs.Sort(new CardComparer());
            threeOfAKind.Sort(new CardComparer());
            fullHouse.Sort(new CardComparer());
            fourOfAKind.Sort(new CardComparer());
            fiveOfAKind.Sort(new CardComparer());

            hands.Clear();
            hands = hands
                .Concat(noType)
                .Concat(onePair)
                .Concat(twoPairs)
                .Concat (threeOfAKind)
                .Concat(fullHouse)
                .Concat(fourOfAKind) 
                .Concat(fiveOfAKind)
                .ToList();
        }

        

        private static void Init(StreamReader sr)
        {
            List<string> handStrings = new List<string>();
            List<int> bids = new List<int>();
            string[] lineValues;
            while (!sr.EndOfStream)
            {
                lineValues = sr.ReadLine().Split();
                handStrings.Add(lineValues[0]);
                bids.Add(int.Parse(lineValues[1]));
            }

            HashSet<char> processedCards = new HashSet<char>();
            int duplicateCount;
            int type;
            for (int i = 0; i < handStrings.Count; i++)
            {
                string handString = handStrings[i];
                type = 0;
                processedCards.Clear();
                for (int j = 0; j < handString.Length; j++)
                {
                    if (processedCards.Contains(handString[j])) continue;
                    duplicateCount = 0;                    
                    for (int k = j; k < handString.Length; k++)
                    {
                        if (handString[k] == handString[j]) duplicateCount++;
                    }
                    switch (duplicateCount)
                    {
                        case 1:
                            type += 10;
                            break;
                        case 2:
                            type += 100;
                            break;
                        case 3:
                            type += 1000;
                            break;
                        case 4:
                            type += 10000;
                            break;
                        case 5:
                            type += 100000;
                            break;
                    }
                    processedCards.Add(handString[j]);
                }
                hands.Add(new Hand(handString, bids[i],type));
            }
        }
    }
}
