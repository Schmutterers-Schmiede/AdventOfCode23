using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day6
{
    public class Day6_Part1
    {
        private static List<int> times;
        private static List<int> distances;
        public static void Run()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day6/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());
            InitList(ref times, sr.ReadLine());
            InitList(ref distances, sr.ReadLine());

            int possibilityCount = 1;
            int waysToWin;
            for (int i = 0; i < times.Count; i++)
            {
                waysToWin = GetNumberOfWaysToWinForRace(times[i], distances[i]);
                possibilityCount *= waysToWin;
                Console.WriteLine($"Race {i}: {waysToWin} ways to win");
            }
            Console.WriteLine();
            Console.WriteLine($"number of possibilities: {possibilityCount}");

        }

        private static int GetNumberOfWaysToWinForRace(int time, int distance)
        {
            List<int> winningChargeTimes = new List<int>();
            int j;
            int chargeTime;
            int speed;

            chargeTime = 0;
            speed = 0;
            while (chargeTime < time)
            {
                chargeTime++;
                speed++;
                if (speed * (time - chargeTime) > distance)
                    winningChargeTimes.Add(chargeTime);
            }
            return winningChargeTimes.Count;
        }

        private static void InitList(ref List<int> list, string line)
        {
            //string afterRegex = Regex.Replace(Regex.Replace(line, "[A-Z][a-z]*: *", ""), " +", " ");           
            list = Array.ConvertAll(Regex.Replace(Regex.Replace(line, "[A-Z][a-z]*: *", ""), " +", " ").Split(' '), int.Parse).ToList();
        }

    }
}

