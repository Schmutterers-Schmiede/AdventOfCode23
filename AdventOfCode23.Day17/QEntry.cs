using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day17
{
    public class QEntry
    {
        public CityBlock Block { get; init; }         
        public CityBlock? ReachedFrom { get; init; }
        public int CostToReach { get; set; }        
        public Directions DirectionToReach { get; set; }
        public int DirectionCount { get; set; }
        public (int X, int Y) Position() { return Block.Position; }

        public QEntry(
            CityBlock block,
            CityBlock? reachedFrom,
            Directions directionToReach, 
            int costToReach, 
            int directionCount)
        {       
            Block = block;
            DirectionToReach = directionToReach;
            CostToReach = costToReach;
            DirectionCount = directionCount;
            ReachedFrom = reachedFrom;
        }
        
        public double CombinedCost() 
        { 
            return CostToReach + Block.Heuristic; 
        }
        
    }
}
