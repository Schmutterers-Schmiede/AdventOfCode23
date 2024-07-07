using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day23.Common
{
    internal class Point2d(int x, int y)
    {
        public Point2d(Point2d other) : this(other.X, other.Y) { }
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public override bool Equals(Object obj)
        {
            if (obj is Point2d other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }
    }
}
