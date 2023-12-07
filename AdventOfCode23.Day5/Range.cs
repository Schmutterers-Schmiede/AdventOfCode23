using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day5
{
    public class Range
    {
        public Range(long from, long to)
        {
            From = from;
            To = to;
        }

        public long From { get; set; }
        public long To { get; set; }
    }
}
