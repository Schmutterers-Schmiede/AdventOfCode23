using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day22;

public class Day22_Part2
{
    static Point3d Nowhere = new(0, 0, 0);
    static Point3d xDir = new(1, 0, 0);
    static Point3d yDir = new(0, 1, 0);
    static Point3d zDir = new(0, 0, 1);

    static Dictionary<short, Brick> bricks = new();
    static Dictionary<short, Brick> candidates = new();
    static short[,,] grid;

    public static void Run()
    {
        Init();
        ApplyGravity();
        ExportGrid();
        //PrintBricks(true);        
        MarkEssentialSupports();
        Console.WriteLine($"Sum of falling bricks: {GetSum()}");
    }

    static int GetSum()
    {
        var essentialBricks = bricks
            .Select(x => x.Value)
            .Where(x => x.IsEssentialSupport)
            .ToList();
        short[,,] gridCopy;
        List<Brick> bricksCopy;
        int sum = 0;
        foreach(var brick in essentialBricks)
        {
            gridCopy = CreateGridCopyWithOutBrick(brick);
            bricksCopy = bricks
                .Where(x => x.Value.Id != brick.Id)
                .OrderBy(x => x.Value.Start.Z)
                .Select(x => new Brick(x.Value))
                .ToList();
            
            sum += ApplyGravityToCopy(gridCopy, bricksCopy);
        }
        return sum;
    }

    

    static int ApplyGravityToCopy(short[,,] gridCopy, List<Brick> brickList)
    {
        int sum = 0;
        foreach (Brick brick in brickList)
        {
            if (CanFall(brick, gridCopy))
            {
                sum++;
                Fall(brick, gridCopy);
            }
        }
        return sum;
    }

        static short[,,] CreateGridCopyWithOutBrick(Brick brick)
    {
        short[,,] copy = new short[grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)];
        for (var z = 0; z < grid.GetLength(0); z++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(2); x++)
                {
                    if (grid[z, y, x] == brick.Id)
                        copy[z, y, x] = 0;
                    else
                        copy[z, y, x] = grid[z, y, x];
                }
            }
        }
        return copy;
    }
        static void MarkEssentialSupports()
    {
        Brick brick;
        foreach( var kvp in bricks)
        {
            brick = kvp.Value;
            List<Brick> supports;
            if(brick.Start.Z > 1)
            {
                supports = GetSupports(brick.Id);
                if(supports.Count == 1)
                {
                    bricks[supports.First().Id].IsEssentialSupport = true;
                }
                supports.Clear();
            }
        }
    }

    static List<Brick> GetSupports(short id) 
    {
        List<Brick> supports = new();
        var brick = bricks[id];
        // if brick is single cube or vertical, return the only possible support
        if (brick.Direction.Equals(Nowhere) || brick.Direction.Equals(zDir))
        {
            supports.Add(bricks[grid[brick.Start.Z - 1, brick.Start.Y, brick.Start.X]]);
            return supports;
        }

        // if horizontal mutli cube brick, look for supports
        var current = new Point3d(brick.Start);
        short gridVal;
        while (true)
        {
            gridVal = grid[current.Z - 1, current.Y, current.X];
            if (gridVal != 0 &&
                !supports.Any(brick => brick.Id == gridVal))
                supports.Add(bricks[gridVal]);

            if (current.Equals(brick.End)) break;

            current.X += brick.Direction.X;
            current.Y += brick.Direction.Y;
            current.Z += brick.Direction.Z;

        }
        return supports;        
    }



    static void ApplyGravity()
    {
        // get bricks ordered by height
        List<Brick> brickList = bricks
            .OrderBy(brickKvp => brickKvp.Value.Start.Z)
            .Select(brickKvp => brickKvp.Value)
            .ToList();

        foreach (var brick in brickList)
        {
            if (CanFall(brick, grid))            
                Fall(brick, grid);            
        }
    }

    static void Fall(Brick brick, short[,,] brickGrid)
    {
        Point3d current = new(brick.Start);
        short scout;
        int newZ = 1;

        // find new Z height
        while (true)
        {
            scout = Convert.ToInt16(current.Z);
            while (brickGrid[scout - 1, current.Y, current.X] == 0)
            {
                scout--;
            }
            if(scout > newZ)
                newZ = scout;

            if(current.Equals(brick.End) || brick.Direction.Equals(zDir)) break;

            current.X += brick.Direction.X;
            current.Y += brick.Direction.Y;
            current.Z += brick.Direction.Z;
        }
        short delta = Convert.ToInt16(brick.Start.Z - newZ);

        // move ids in grid
        current = new(brick.Start);
        while (true)
        {
            brickGrid[current.Z, current.Y, current.X] = 0;
            brickGrid[current.Z - delta, current.Y, current.X] = brick.Id;

            if (current.Equals(brick.End)) break;

            current.X += brick.Direction.X;
            current.Y += brick.Direction.Y;
            current.Z += brick.Direction.Z;
        }
    

        brick.Start.Z = Convert.ToInt16(brick.Start.Z - delta);
        brick.End.Z = Convert.ToInt16(brick.End.Z - delta);
    }


    static bool CanFall(Brick brick, short[,,] brickGrid) 
    {                
        // vertical or single cube bricks
        if(brick.Direction.Equals(zDir) || brick.Direction.Equals(Nowhere))
            return brickGrid[brick.Start.Z - 1, brick.Start.Y, brick.Start.X] == 0;

        // horizontal multi cube bricks
        var current = new Point3d(brick.Start);
        while (true)
        {
            if (brickGrid[current.Z - 1, current.Y, current.X] != 0)
                return false;

            if (current.Equals(brick.End)) break;

            current.X += brick.Direction.X;
            current.Y += brick.Direction.Y;            
        }
        return true;
    }

    // for visualization with https://array-3d-viz.vercel.app/
    static void ExportGrid()
    {        
        var sb = new StringBuilder();
        sb.Append('[');
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            sb.Append('[');
            for (int z = grid.GetLength(0) - 1; z >= 0; z--)
            {
                sb.Append('[');
                for (int x = 0; x < grid.GetLength(2); x++)
                {
                    sb.Append(Convert.ToString(grid[z,y,x]));
                    if(x < grid.GetLength(2) - 1)
                        sb.Append(',');
                }
                sb.Append(']');
                if (z > 0)
                    sb.Append(',');
            }
            sb.Append(']');
            if(y < grid.GetLength(1) - 1)
                sb.Append(',');
        }
        sb.Append(']');

        File.WriteAllText("grid.txt", sb.ToString());
    }

    static void Init()
    {
        var input = File.ReadAllLines("Common/input22.txt");
        short[] coords = new short[6];
        short xMax = 0;
        short yMax = 0;
        short zMax = 0;

        for (int i = 0; i < input.Length; i++)
        {            
            // get coords from input line
            var coordStrings = input[i].Split(',', '~');
            coords = [
                short.Parse(coordStrings[0]),
                short.Parse(coordStrings[1]),
                short.Parse(coordStrings[2]),
                short.Parse(coordStrings[3]),
                short.Parse(coordStrings[4]),
                short.Parse(coordStrings[5])
            ];

            // calculate grid boundries
            if (coords[0] > xMax || coords[3] > xMax)
                xMax = Math.Max(coords[0], coords[3]);

            if (coords[1] > yMax || coords[4] > yMax)
                yMax = Math.Max(coords[1], coords[4]);

            if (coords[2] > zMax || coords[5] > zMax)
                zMax = Math.Max(coords[2], coords[5]);

            var point1 = new Point3d(coords[0], coords[1], coords[2]);
            var point2 = new Point3d(coords[3], coords[4], coords[5]);

            // make sure start always has the lowest coordinates for iteration
            if (point1.Z != point2.Z)
            {
                if(point1.Z < point2.Z)
                    bricks.Add((short)(i + 1), new((short)(i + 1), point1, point2));
                else 
                    bricks.Add((short)(i + 1), new((short)(i + 1), point2, point1));
            }

            else if (point1.Y != point2.Y)
            {
                if (point1.Y < point2.Y)
                    bricks.Add((short)(i + 1), new((short)(i + 1), point1, point2));
                else
                    bricks.Add((short)(i + 1), new((short)(i + 1), point2, point1));
            }

            else if (point1.X != point2.X)
            {
                if (point1.X < point2.X)
                    bricks.Add((short)(i + 1), new((short)(i + 1), point1, point2));
                else
                    bricks.Add((short)(i + 1), new((short)(i + 1), point2, point1));            
            }
            else // single cube brick
            {
                bricks.Add((short)(i + 1), new((short)(i + 1), point1, point2));
            }
        }

        grid = new short[zMax + 1, yMax + 1, xMax + 1];

        // init floor 
        for (short y = 0; y < grid.GetLength(1); y++)
            for (short x = 0; x < grid.GetLength(2); x++)
                grid[0, y, x] = -1;

        // set brick positions in grid
        Brick brick;
        foreach(var brickKvp in bricks)
        {
            brick = brickKvp.Value;
        
            for (var i = brick.Start.X; i <= brick.End.X; i++) 
                grid[brick.Start.Z, brick.Start.Y, i] = brick.Id;

            for (var i = brick.Start.Y; i <= brick.End.Y; i++)
                grid[brick.Start.Z, i, brick.Start.X] = brick.Id;

            for (var i = brick.Start.Z; i <= brick.End.Z; i++)
                grid[i, brick.Start.Y, brick.Start.X] = brick.Id;            
        }
    }
}
