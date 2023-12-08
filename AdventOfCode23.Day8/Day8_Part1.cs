using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day8
{
    public class Day8_Part1
    {
        private static Dictionary<string,Node> map = new Dictionary<string,Node>();
        private static string directions;
        public static void Run()
        {
            Init();
            int steps = CountSteps();
            Console.WriteLine($"step count: {steps}");

        }

        private static int CountSteps()
        {
            int steps = 0;
            string currentNodeId = "AAA";
            int directionIndex = 0;

            while(currentNodeId != "ZZZ")
            {
                if( directionIndex >= directions.Length ) directionIndex = 0;

                Console.Write($"{currentNodeId} {directions[directionIndex]} ");
                if (directions[directionIndex] == 'L')                
                    currentNodeId = map[currentNodeId].Left;                
                else
                    currentNodeId = map[currentNodeId].Right;

                steps++;
                directionIndex++;
                Console.WriteLine($"{currentNodeId}");
            }
            Console.WriteLine();
            return steps;
        }

        private static void Init()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day8/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());

            string line;
            string nodeId;
            string[] nodeProperties;
            string[] substringBuffer;
            directions = sr.ReadLine();
            while(!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Length == 0) continue;

                substringBuffer = line.Split(" = ");

                nodeId = substringBuffer[0];
                nodeProperties = Regex.Replace(substringBuffer[1], "[\\(),]", "").Split();

                map.Add(nodeId, new Node(nodeProperties[0], nodeProperties[1]));
            }
        }
    }
}
