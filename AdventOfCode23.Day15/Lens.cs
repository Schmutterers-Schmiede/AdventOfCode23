using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day15
{
    public class Lens
    {
        public int FocalLength { get; }
        public string Label { get; set; }

        public Lens(string label, int focalLength)
        {
            Label = label;
            FocalLength = focalLength;
        }
    }
}
