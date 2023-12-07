using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode23.Day6
{
    public class Day6_Part2
    {
        private static long time;
        private static long distance;
        public static void Run()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day6/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());
            time = GetNumberFromString(sr.ReadLine());
            distance = GetNumberFromString(sr.ReadLine());
                        
            
            int waysToWin = GetNumberOfWaysToWinForRace(time, distance);            
                
            
            Console.WriteLine();
            Console.WriteLine($"number of possibilities: {waysToWin}");

        }

        private static int GetNumberOfWaysToWinForRace(long time, long distance)
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

        private static long GetNumberFromString(string line)
        {
            string afterRegex = Regex.Replace(Regex.Replace(line, "[A-Z][a-z]*: *", ""), " +", "");           
            return long.Parse(afterRegex);
        }
    }
}
