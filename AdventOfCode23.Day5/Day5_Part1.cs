using System.Collections.Concurrent;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day5;

public class Day5_Part1
{
    private static string[] mapNames =
        {
            "seed_soil",
            "soil_fertilizer",
            "fertilizer_water",
            "water_light",
            "light_temperature",
            "temperature_humidity",
            "humidity_location"
        };

    private static string[] stages =
        {
            "soil",
            "fertilizer",
            "water",
            "light",
            "temperature",
            "humidity",
            "location"
        };
    private static List<List<MapRow>> maps = new List<List<MapRow>>();
    private static long[] seeds;
    public static void Run()
    {
        StringBuilder path = new StringBuilder();
        path.Append(AppDomain.CurrentDomain.BaseDirectory);
        path.Append("../../../../AdventOfCode23.Day5/input1.txt");
        StreamReader sr = new StreamReader(path.ToString());

        seeds = Array.ConvertAll(Regex.Replace(sr.ReadLine(), "seeds: ", "").Split(' '), long.Parse);

        PopulateMaps(sr);
        //PrintMaps();
        Console.WriteLine($"Nearest location id: {FindNearestLocation()}");



    }

    private static void PrintMaps()
    {

        for (int i = 0; i < maps.Count; i++)
        {
            Console.WriteLine($"{mapNames[i]}:");
            foreach (var row in maps[i])
            {
                Console.WriteLine($"{row.Destination.From} {row.Source.From} {row.Destination.To - row.Destination.From}");
            }
            Console.WriteLine();
        }
    }

    private static long FindNearestLocation()
    {
        long minLocationId = long.MaxValue;
        long Id;
        foreach (var seed in seeds)
        {
            Console.Write($"seed: {seed}");
            Id = seed;
            for (int i = 0; i < maps.Count; i++)
            {

                foreach (var row in maps[i])
                {
                    if (IsInRange(Id, row.Source.From, row.Source.To))
                    {
                        Id = row.Destination.From + (Id - row.Source.From);
                        break;
                    }
                }
                Console.Write($" {stages[i]}: {Id} ");
            }
            if (Id < minLocationId)
                minLocationId = Id;
            Console.WriteLine("\n");
        }
        return minLocationId;
    }

    private static bool IsInRange(long value, long from, long to)
    {
        return value >= from && value <= to;
    }

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
