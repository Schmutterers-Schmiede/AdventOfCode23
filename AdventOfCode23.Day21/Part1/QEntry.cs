using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day21
{
    internal class QEntry(Point pos, int steps)
    {
        public Point Pos { get; init; } = pos;
        public int Steps { get; init; } = steps;
    }
}
