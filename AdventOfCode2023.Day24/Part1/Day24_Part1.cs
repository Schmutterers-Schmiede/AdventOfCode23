using MathNet.Spatial.Euclidean;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AdventOfCode23.Day24;

public class Day24_Part1
{
    static List<Line2D> lines = new();
    static List<Point2D> origins = new();
    static List<Point2D> velocities = new();
    static HashSet<(Line2D, Line2D)> processedPairs = new();

    //static int testAreaLowerbound = 7;
    //static int testAreaUpperbound = 27;

    static int intersections = 0;
    static long testAreaUpperbound = 400000000000000;
    static long testAreaLowerbound = 200000000000000;


    public static void Run()
    {
        Init();
        CountIntersections();
        Console.WriteLine($"number of intersections in test area: {intersections}");
    }
    Point2D test;
    static void CountIntersections()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            {
                for (int j = i + 1; j < lines.Count; j++)
                {
                    var intersection = lines[i].IntersectWith(lines[j]);
                    if (intersection is not null &&
                        intersection.Value.X <= testAreaUpperbound &&
                        intersection.Value.X >= testAreaLowerbound &&
                        intersection.Value.Y <= testAreaUpperbound &&
                        intersection.Value.Y >= testAreaLowerbound)
                    {
                        var controlLine1 = new Line2D(lines[i].StartPoint, intersection.Value);
                        var controlLine2 = new Line2D(lines[j].StartPoint, intersection.Value);
                        if (controlLine1.Direction.Equals(lines[i].Direction, 0.0001) &&
                            controlLine2.Direction.Equals(lines[j].Direction, 0.0001))
                            intersections++;
                    }
                }
            }
        }
    }
    static void Init()
    {
        var input = File.ReadAllLines("Common/input24.txt");
        foreach (var inputLine in input)
        {
            var coordStrings = inputLine.Split(" @");
            var originCoordStrings = coordStrings[0].Split(", ");
            var velocityCoordStrings = coordStrings[1].Split(", ");

            var x = Convert.ToDouble(originCoordStrings[0]);
            var y = Convert.ToDouble(originCoordStrings[1]);
            var vx = Convert.ToDouble(velocityCoordStrings[0]);
            var vy = Convert.ToDouble(velocityCoordStrings[1]);

            var lineStart = new Point2D(x, y);
            var lineEnd = new Point2D(x + vx, y + vy);
            lines.Add(new(lineStart, lineEnd));
        }
    }

}
