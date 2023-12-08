using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode23.Day8
{
    public class Day8_Part2
    {
        private static string directions;
        private static Dictionary<string, Node> map = new Dictionary<string, Node>();
        private static List<string> pathNodes = new List<string>();
        private static long[] roundTripLengths;
        public static void Run()
        {
            Init();    
            long steps = CountSteps();
            Console.WriteLine();
            Console.WriteLine($"total steps: {steps}");            
        }

        private static void step(int pathNodeIndex, char direction)
        {
            if(direction == 'L')             
                pathNodes[pathNodeIndex] = map[pathNodes[pathNodeIndex]].Left;            

            else           
                pathNodes[pathNodeIndex] = map[pathNodes[pathNodeIndex]].Right;            
        }

        private static long CountSteps()
        {
            int steps = 0;            
            int directionIndex;

            for(int i = 0; i < pathNodes.Count; i++)
            {
                directionIndex = 0;
                steps = 0;
                while (pathNodes[i].Last() != 'Z')
                {
                    step(i, directions[directionIndex]);
                    directionIndex++;
                    if (directionIndex >= directions.Length) directionIndex = 0;
                    steps++;
                }
                roundTripLengths[i] = steps;
            }

            Console.WriteLine($"round trip lengths:");
            foreach(int item in roundTripLengths)
            {
                Console.WriteLine(item);
            }
            
            return CalculateLCM(roundTripLengths);
        }

        private static long CalculateGCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private static long CalculateLCM(long[] numbers)
        {
            long lcm = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
            {
                long gcd = CalculateGCD(lcm, numbers[i]);
                lcm = (lcm * numbers[i]) / gcd;
            }

            return lcm;
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
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Length == 0) continue;
                
                substringBuffer = line.Split(" = ");

                nodeId = substringBuffer[0];
                if(nodeId.Last() == 'A') pathNodes.Add(nodeId);

                nodeProperties = Regex.Replace(substringBuffer[1], "[\\(),]", "").Split();

                map.Add(nodeId, new Node(nodeProperties[0], nodeProperties[1]));
            }
            roundTripLengths = new long[pathNodes.Count]; 
        }
    }
}
