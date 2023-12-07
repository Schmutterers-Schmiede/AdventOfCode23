using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day5
{
    public class MapRow
    {
        public MapRow(long destinationRangeStart, long sourceRangeStart, long rangeLength)
        {            
            Source = new Range(sourceRangeStart, sourceRangeStart + rangeLength - 1);
            Destination = new Range(destinationRangeStart, destinationRangeStart + rangeLength - 1);
        }
        public Range Source { get; set; }
        public Range Destination {  get; set; }        
    }
}
