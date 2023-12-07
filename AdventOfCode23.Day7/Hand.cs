using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day7
{
    public class Hand
    {
        public int Type { get; set; }
        public string Cards { get; set; }
        public int Bid { get; set; }

        public Hand(string cards, int bid, int type)
        {
            Cards = cards;
            Bid = bid;
            Type = type;
        }
    }
}
