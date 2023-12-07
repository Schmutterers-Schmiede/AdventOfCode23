using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day7;

public class Day7_Part2
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

    private static List<int> types = new List<int>
    {
        50,
        130,
        210,
        1020,
        1100,
        10010,
        100000
    };

    private static int ApplyJokersToType(int type, int jokerCount, string hand)
    {
        int result = type;
        for (int i = 0; i < jokerCount; i++)
        {
            if (jokerCount == 5) return 100000;
            switch(result)
            {
                case 50:
                    result = 130;
                    break;
                case 130:
                    result = 1020;
                    break;
                case 210:
                    result = 1100;
                    break;
                case 1020:
                    result = 10010;
                    break;
                case 10010:
                    result = 100000;
                    break;
                default:
                    throw new Exception("problem converting type");
            }
        }
        return result;
    }

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
        List<Hand> noType = hands.Where(hand => hand.Type == 50).ToList();
        List<Hand> onePair = hands.Where(hand => hand.Type == 130).ToList();
        List<Hand> twoPairs = hands.Where(hand => hand.Type == 210).ToList();
        List<Hand> threeOfAKind = hands.Where(hand => hand.Type == 1020).ToList();
        List<Hand> fullHouse = hands.Where(hand => hand.Type == 1100).ToList();
        List<Hand> fourOfAKind = hands.Where(hand => hand.Type == 10010).ToList();
        List<Hand> fiveOfAKind = hands.Where(hand => hand.Type == 100000).ToList();        

        noType.Sort(new CardComparerWithJoker());
        onePair.Sort(new CardComparerWithJoker());
        twoPairs.Sort(new CardComparerWithJoker());
        threeOfAKind.Sort(new CardComparerWithJoker());
        fullHouse.Sort(new CardComparerWithJoker());
        fourOfAKind.Sort(new CardComparerWithJoker());
        fiveOfAKind.Sort(new CardComparerWithJoker());

        hands.Clear();
        hands = hands
            .Concat(noType)
            .Concat(onePair)
            .Concat(twoPairs)
            .Concat(threeOfAKind)
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
        int jokerCount;
        for (int i = 0; i < handStrings.Count; i++)
        {
            string handString = handStrings[i];
            type = 0;
            jokerCount = 0;
            processedCards.Clear();
            for (int j = 0; j < handString.Length; j++)
            {
                if (processedCards.Contains(handString[j])) continue;
                duplicateCount = 0;
                if (handString[j] == 'J')
                {
                    jokerCount++;
                    type += 10;
                }
                for (int k = j; k < handString.Length; k++)
                {
                    if (handString[k] == 'J') continue;
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

                if (handString[j] != 'J')processedCards.Add(handString[j]);
            }
            if(jokerCount > 0)
            {
                type = ApplyJokersToType(type, jokerCount, handString);
            }
            hands.Add(new Hand(handString, bids[i], type));
        }
    }
}
