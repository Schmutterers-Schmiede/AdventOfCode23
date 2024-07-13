using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day25;

internal class Node(string id)
{
    public string Id { get; set; } = id;
    public List<String> Edges { get; set; } = new();
}
