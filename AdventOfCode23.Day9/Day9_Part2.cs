using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day9
{
    public class Day9_Part2
    {
        private static List<Reading> readings = new List<Reading>();
        public static void Run()
        {
            Init();
            Console.WriteLine($"sum of predicted values: {SumOfPredictedValues()}");
        }

        private static int SumOfPredictedValues()
        {
            int result = 0;
            foreach (Reading reading in readings)
            {
                result += PredictNextValue(reading);
            }
            return result;
        }

        private static int PredictNextValue(Reading reading)
        {
            int[] result;
            List<int> diffs = new List<int>();

            for (int i = 1; i < reading.Values.Length; i++)
            {
                diffs.Add(reading.Values[i] - reading.Values[i - 1]);
            }
            reading.Differences.Add(diffs.ToArray());

            do
            {
                diffs.Clear();
                for(int j = 1; j < reading.Differences.Last().Length; j++)
                {
                    diffs.Add(reading.Differences.Last()[j] - reading.Differences.Last()[j - 1]);
                }                
                reading.Differences.Add(diffs.ToArray());
            }
            while (!diffs.All(x => x == 0));

            int previousPredictionValue = 0;
            for (int i = reading.Differences.Count - 2; i >= 0; i--)
            {
                previousPredictionValue = reading.Differences[i].First() - previousPredictionValue;                
            }

            return reading.Values.First() - previousPredictionValue;
        }

        private static void Init()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day9/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());

            while(!sr.EndOfStream)
            {
                int[] line = Array.ConvertAll(sr.ReadLine().Split(), int.Parse);
                readings.Add(new Reading(line));
            }    
        }
    }
}
