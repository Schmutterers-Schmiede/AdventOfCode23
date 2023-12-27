using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day15
{
    public class Box
    {
        public List<Lens> Lenses { get; set; }

        public Box() 
        {
            Lenses = new List<Lens>();
        }
    }
}
