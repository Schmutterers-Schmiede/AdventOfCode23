using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode23.Day5
{
    public class Day5_Part2
    {
        //private static string[] mapNames =
        //{
        //    "seed_soil",
        //    "soil_fertilizer",
        //    "fertilizer_water",
        //    "water_light",
        //    "light_temperature",
        //    "temperature_humidity",
        //    "humidity_location"
        //};

        //private static string[] stages =
        //    {
        //    "soil",
        //    "fertilizer",
        //    "water",
        //    "light",
        //    "temperature",
        //    "humidity",
        //    "location"
        //};
        private static List<List<MapRow>> maps = new List<List<MapRow>>();
        private static long[] seedInput;
        private static List<Range> seedRanges = new List<Range>();
        public static void Run()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day5/input2.txt");
            StreamReader sr = new StreamReader(path.ToString());

            InitSeedRanges(sr.ReadLine());

            PopulateMaps(sr);
            //PrintMaps(); 

            Console.WriteLine($"nearest location id: {FindNearestLocationId()}"); 

        }
        
        private static long FindNearestLocationId()
        {
            List<Range> newRangeBuffer = new List<Range>();
            bool notMapped;
            foreach (var map in maps)
            {
                foreach(var range in seedRanges)
                {
                    notMapped = true;
                    foreach (var row in map)
                    {
                        if(IsBetween(range.From, row.Source.From, row.Source.To) &&
                            IsBetween(range.To, row.Source.From, row.Source.To))
                        {
                            newRangeBuffer.Add(TranslateRange(range, row));                            
                            notMapped = false;
                            break;
                        }
                        else if(IsBetween(range.From, row.Source.From, row.Source.To))
                        {
                            newRangeBuffer.Add(TranslateRange(new Range(range.From, row.Source.To), row));
                            range.From = row.Source.To + 1;
                            newRangeBuffer.Add(range);
                            notMapped = false;
                        }
                        else if(IsBetween(range.To, row.Source.From, row.Source.To))
                        {
                            newRangeBuffer.Add(TranslateRange(new Range(row.Source.From, range.To), row));
                            range.To = row.Source.From - 1;
                            newRangeBuffer.Add(range);
                            notMapped = false;
                        }
                    }
                    if (notMapped)
                    {
                        newRangeBuffer.Add(range);
                    }
                }
                seedRanges.Clear();
                seedRanges = seedRanges.Concat(newRangeBuffer).ToList();
                newRangeBuffer.Clear();
            }

            long minLocationId = long.MaxValue;
            foreach(var range in seedRanges)
            {
                if(range.From < minLocationId) minLocationId = range.From;
            }
            return minLocationId;
        }

        private static Range TranslateRange(Range range, MapRow row)
        {
            long delta = row.Destination.From - row.Source.From;
            return new Range(
                range.From + delta,
                range.To + delta
            );
        }
        
        private static void InitSeedRanges(string line) {
            seedInput = Array.ConvertAll(Regex.Replace(line, "seeds: ", "").Split(' '), long.Parse);
            for (int i = 0; i < seedInput.Length; i += 2)
            {
                seedRanges.Add(new Range(seedInput[i], seedInput[i] + seedInput[i + 1] - 1));
            }
        }

        

        private static bool IsBetween(long value, long lower, long upper)        
        => (value >= lower && value <= upper);
                       
        private static void PopulateMaps(StreamReader sr)
        {
            for (int i = 0; i < 7; i++)
            {
                maps.Add(new List<MapRow>());
            }

            int mapIndex = -1;
            string line;
            long[] valueBuffer = new long[3];
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Length == 0) { continue; }
                if (char.IsLetter(line[0]))
                {
                    mapIndex++;
                }
                else if (char.IsDigit(line[0]))
                {
                    valueBuffer = Array.ConvertAll(line.Split(' '), long.Parse);
                    maps[mapIndex].Add(new MapRow(valueBuffer[0], valueBuffer[1], valueBuffer[2]));
                }
            }
        }
    }
}
