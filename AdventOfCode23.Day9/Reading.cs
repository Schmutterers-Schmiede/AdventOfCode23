using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day9
{
    internal class Reading
    {
        public int[] Values { get; set; }
        public List<int[]> Differences {  get; set; }
        public List<int> NextValues { get; set; }

        public Reading(int[] values)
        {
            Values = values;            
            Differences = new List<int[]>();    
            NextValues = new List<int>();
        }
    }
}
