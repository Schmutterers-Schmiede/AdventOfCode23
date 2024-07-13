using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day24;

internal record Hailstone(long X, long Y, long Z, long Vx, long Vy, long Vz)
{
    public Hailstone RelativeTo(Hailstone source) => new Hailstone(X - source.X, Y - source.Y, Z - source.Z, Vx - source.Vx, Vy - source.Vy, Vz - source.Vz);
}
