namespace AdventOfCode23.Day17.Common
{
    public class CityBlock
    {
        public (int X, int Y) Position { get; init; }
        public int HeatLoss { get; init; }
        public double Heuristic { get; init; }

        public Dictionary<Directions, Edge> Edges = new();


        public CityBlock(int heatLoss, double heuristic, int x, int y)
        {
            Position = (x, y);
            HeatLoss = heatLoss;
            Heuristic = heuristic;
        }

        public bool Equals(CityBlock other)
        {
            return other.Position == Position;
        }
    }
}
