using System.Text;

namespace AdventOfCode.Day12
{
    public class Day12_Part1
    {
        public static void Run()
        {
            
        }

        public static void Init()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day12/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());


        }

    }
}
