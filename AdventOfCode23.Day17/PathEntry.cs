using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day17
{
    public class PathEntry
    {        
        public CityBlock Block { get; init; }        
        public PathEntry Previous { get; init; }        

        public PathEntry(CityBlock block, PathEntry? previous)
        {
            Block = block;
            Previous = previous;
        }
    }
}
