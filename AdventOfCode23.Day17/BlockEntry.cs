namespace AdventOfCode23.Day17
{
    public class BlockEntry
    {
        public BlockEntry(
            string id,
            CityBlock block,
            string previousBlockId,
            int costToReach,
            Directions directionToReach,
            int directionCount)
        {
            Id = id;
            Block = block;
            PreviousBlockId = previousBlockId;            
            CostToReach = costToReach;
            DirectionToReach = directionToReach;
            DirectionCount = directionCount;
        }
        public string Id { get; set; }
        public CityBlock Block { get; set; }
        public string PreviousBlockId { get; set; }
        public int CostToReach { get; set; }

        public Directions DirectionToReach { get; set; }
        public int DirectionCount { get; set; }

        public double CombinedCost() { return CostToReach + Block.Heuristic; }
    }
}
