using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day19.Part2;

internal class PartRange(
    IntRange x, IntRange m, IntRange a, IntRange s)
{
    public IntRange X { get; set; } = x;
    public IntRange M { get; set; } = m;
    public IntRange A { get; set; } = a;
    public IntRange S { get; set; } = s;
}
