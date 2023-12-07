using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day7
{
    public class TypeComparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            return x.Type.CompareTo(y.Type);
        }
    }
}
