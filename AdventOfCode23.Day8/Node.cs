using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day8
{
    public class Node
    {        
        public string Left { get; set; }
        public string Right { get; set; }

        public Node(string left, string right)
        {     
            Left = left;
            Right = right;
        }
    }
}
