using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day17.Common
{
    public class Edge
    {
        public CityBlock Target { get; init; }

        public Edge(CityBlock target)
        {
            Target = target;
        }
    }
}
