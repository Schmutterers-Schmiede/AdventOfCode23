using AdventOfCode23.Day23.Common;

namespace AdventOfCode23.Day23
{
    internal class Walker(Point2d position, Point2d direction, int stepsTaken, List<Point2d> path)
    {
        public Point2d Position { get; set; } = position;
        public Point2d Direction { get; set; } = direction;
        public int StepsTaken { get; set; } = stepsTaken;
        public List<Point2d> Path { get; set; } = path;

        public void Move()
        {
            Position.X += Direction.X;
            Position.Y += Direction.Y;
            StepsTaken++;
        }
    }
}
