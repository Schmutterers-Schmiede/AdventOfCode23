using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day7
{
    public class CardComparerWithJoker : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            int comp;
            for (int i = 0; i < x.Cards.Length; i++)
            {
                comp = charToCardValue(x.Cards[i]).CompareTo(charToCardValue(y.Cards[i]));
                if (comp == 0) continue;
                else return comp;
            }
            return 0;
        }

        private int charToCardValue(char c)
        {
            switch (c)
            {
                case 'J':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'T':
                    return 10;
                case 'Q':
                    return 11;
                case 'K':
                    return 12;
                case 'A':
                    return 13;
                default:
                    return -1;
            }
        }
    }
}
