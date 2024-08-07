﻿using MathNet.Spatial.Euclidean;

namespace AdventOfCode23.Day24;

public class Day24_Part2
{
    //var testAreaMin = 7;
    //var testAreaMax = 27;
    static long testAreaMin = 200000000000000;
    static long testAreaMax = 400000000000000;
    static List<Hailstone> hailstones = new();

    static double x;
    static double y;
    static double z;

    public static void Run()
    {
        Init();
        CalculateRockPosition();
        Console.WriteLine($"initial rock position: {x + y + z}");
    }

    static void CalculateRockPosition()
    {
        var hailstone1 = hailstones[1].RelativeTo(hailstones[0]);
        var hailstone2 = hailstones[2].RelativeTo(hailstones[0]);

        var p1 = new Vector3D(hailstone1.X, hailstone1.Y, hailstone1.Z);
        var p2 = new Vector3D(hailstone2.X, hailstone2.Y, hailstone2.Z);
        var v1 = new Vector3D(hailstone1.Vx, hailstone1.Vy, hailstone1.Vz);
        var v2 = new Vector3D(hailstone2.Vx, hailstone2.Vy, hailstone2.Vz);

        var t1 = -((p1.CrossProduct(p2)) * v2) / (v1.CrossProduct(p2) * v2);
        var t2 = -((p1.CrossProduct(p2)) * v1) / (p1.CrossProduct(v2) * v1);

        var absolutePosition1 = new Vector3D(hailstones[1].X, hailstones[1].Y, hailstones[1].Z);
        var absolutePosition2 = new Vector3D(hailstones[2].X, hailstones[2].Y, hailstones[2].Z);
        var absoluteVelocity1 = new Vector3D(hailstones[1].Vx, hailstones[1].Vy, hailstones[1].Vz);
        var absoluteVelocity2 = new Vector3D(hailstones[2].Vx, hailstones[2].Vy, hailstones[2].Vz);
        var absoluteCollision1 = absolutePosition1 + t1 * absoluteVelocity1;
        var absoluteCollision2 = absolutePosition2 + t2 * absoluteVelocity2;

        var velocity = (absoluteCollision2 - absoluteCollision1) / (t2 - t1);
        var position = absoluteCollision1 - t1 * velocity;
        x = position.X;
        y = position.Y;
        z = position.Z;
    }

    static void Init()
    {
        var input = File.ReadAllLines("Common/input24.txt");
        foreach (var line in input)
        {
            var parts = line.Split('@', StringSplitOptions.TrimEntries);
            var positions = parts[0].Split(",", StringSplitOptions.TrimEntries);
            var velocities = parts[1].Split(",", StringSplitOptions.TrimEntries);
            var x = long.Parse(positions[0]);
            var y = long.Parse(positions[1]);
            var z = long.Parse(positions[2]);

            var vx = long.Parse(velocities[0]);
            var vy = long.Parse(velocities[1]);
            var vz = long.Parse(velocities[2]);


            hailstones.Add(new Hailstone(x, y, z, vx, vy, vz));
        }
    }
}
