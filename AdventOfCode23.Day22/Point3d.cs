using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day22
{
    internal class Point3d(short x, short y, short z)
    {
        public Point3d(Point3d other) : this(other.X, other.Y, other.Z) { }
        public short X { get; set; } = x;
        public short Y { get; set; } = y;
        public short Z { get; set; } = z;        

        public bool Equals(Point3d other) 
            => X == other.X && Y == other.Y && Z == other.Z;
    }
}
