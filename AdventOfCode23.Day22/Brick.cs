using System.ComponentModel;
using System.Drawing;
using System.Dynamic;

namespace AdventOfCode23.Day22
{
    internal class Brick(short id, Point3d start, Point3d end)
    {
        public short Id { get; set; } = id;        
        public Point3d Start { get; set; } = start;
        public Point3d End { get; set; } = end;        
        public Point3d Direction { get; set; } = GetDirection(start, end);
        public bool IsEssentialSupport { get; set; }

        private static Point3d GetDirection(Point3d start, Point3d end)
        {
            if (start.X < end.X)
                return new Point3d(1, 0, 0);
            if (start.Y < end.Y)
                return new Point3d(0, 1, 0);
            if (start.Z < end.Z)
                return new Point3d(0, 0, 1);

            return new Point3d(0, 0, 0);
        }
    }
}
