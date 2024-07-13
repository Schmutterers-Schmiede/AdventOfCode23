using System.Text.RegularExpressions;

namespace AdventOfCode23.Day25;

public class Day25_Part1
{
    static Dictionary<string, Node> nodes = new();
    static List<string> visited = new();
    static Queue<string> unvisited = new();
   
    public static void Run()
    {
        Init();
        Console.WriteLine(SevereAndCalculate());
    }

    static int SevereAndCalculate()
    {
        // sample input
        //CutConection("cmg", "bvb");
        //CutConection("hfx", "pzl");
        //CutConection("jqt", "nvd");       
        //unvisited.Enqueue("cmg");

        // actual input
        CutConection("xxq", "hqq");
        CutConection("kgl", "xzz");
        CutConection("qfb", "vkd");
        unvisited.Enqueue("vkd");

        // BFS
        Node currentNode;
        int count = 0;
        while (unvisited.Count > 0) {
            currentNode = nodes[unvisited.Dequeue()];
            foreach (var id in currentNode.Edges) 
            {
                if(!visited.Contains(id) && !unvisited.Contains(id)) 
                    unvisited.Enqueue(id);
            }
            visited.Add(currentNode.Id);
            count++;
        }
        return (nodes.Count - count) * count;
    }

    static void CutConection(string a, string b)
    {
        nodes[a].Edges.Remove(b);
        nodes[b].Edges.Remove(a);
    }

    static void Init()
    {
        var input = File.ReadAllLines("Common/input25.txt");
        string nodeId;
        string[] inputLine;
        string[] connections;

        string[] graphVizFileDump = new string[input.Length];
        string graphVizLine;

        for(int i = 0; i < input.Length; i++)
        {
            inputLine = input[i].Split(": ");
            nodeId = inputLine[0];
            if (!nodes.ContainsKey(nodeId))
                nodes.Add(nodeId, new(nodeId));

            connections = inputLine[1].Split(" ");
            graphVizLine = Regex.Replace(input[i], ": ", " -- ");
            graphVizLine = Regex.Replace(graphVizLine, "(?<=[a-z]) (?=[a-z])", ",");
            graphVizFileDump[i] = graphVizLine;                
                       
            for(int j = 0; j < connections.Length; j++)
            {
                if (!nodes.ContainsKey(connections[j]))
                    nodes.Add(connections[j], new(connections[j]));

                if (!nodes[nodeId].Edges.Contains(connections[j]))
                {
                    nodes[nodeId].Edges.Add(connections[j]);
                    nodes[connections[j]].Edges.Add(nodeId);
                }
            }
        }

        File.WriteAllLines("Common/graphVizInput.txt", graphVizFileDump);

    }
}
