using AdventOfCode23.Day23.Common;
namespace AdventOfCode23.Day23;

public static class Day23_Part2
{
    static int height;
    static int width;
    static char[,] grid;
    static Walker lastWalker;
    static char[] directionChars = { '<', '>', '^', 'v' };

    static Point2d up = new Point2d(0, 1);
    static Point2d down = new Point2d(0, -1);
    static Point2d left = new Point2d(-1, 0);
    static Point2d right = new Point2d(1, 0);

    static Point2d start;
    static Point2d end;

    public static void Run()
    {
        Init();
        PrintGrid();
        Console.WriteLine();
        Walk();
        Console.WriteLine();
        PrintLongestPath();
        Console.WriteLine();
        Console.WriteLine($"Longest path length: {lastWalker.StepsTaken}");
    }

    static void Walk()
    {
        Walker initialWalker = new Walker(new(start), down, 0, new());
        initialWalker.Path.Add(new(start));

        Walker newWalker;
        List<Walker> activeWalkers = new() { initialWalker };
        List<Walker> newWalkers = new();
        List<Walker> walkersToBeRemoved = new();

        List<Point2d> spawnDirections;

        while (activeWalkers.Count != 0)
        {
            foreach (Walker walker in activeWalkers)
            {
                if (walker.Path.Contains(new(walker.Position.X + walker.Direction.X, walker.Position.Y + walker.Direction.Y)))
                    walkersToBeRemoved.Add(walker);

                if (!walker.CanMove())
                    walker.ChangeDirection();

                walker.Move();
                walker.Path.Add(new(walker.Position));

                // check if goal reached
                if (walker.Position.Equals(end))
                {
                    walkersToBeRemoved.Add(walker);

                    if (lastWalker is null || walker.StepsTaken > lastWalker.StepsTaken)
                        lastWalker = walker;

                    continue;
                }

                if(walker.Position.Equals(start))
                {
                    walkersToBeRemoved.Add(walker);
                    continue;
                }

                

                // split walker into multiple that each go in a different direction
                if (walker.Position.IsNode())
                {
                    spawnDirections = GetSpawnDirections(walker.Position, walker.Direction);
                    foreach (var direction in spawnDirections)
                    {
                        Point2d spawnPos = new(walker.Position.X + direction.X, walker.Position.Y + direction.Y);
                        newWalker = new(spawnPos, direction, walker.StepsTaken + 1, CreatePathCopy(walker.Path));
                        newWalker.Path.Add(new(newWalker.Position));
                        newWalkers.Add(newWalker);
                    }
                    walkersToBeRemoved.Add(walker);
                }
            }
            Console.WriteLine($"current numebr of walkers: {activeWalkers.Count}");

            foreach (var walker in newWalkers)
                activeWalkers.Add(walker);

            foreach (var walker in walkersToBeRemoved)
                activeWalkers.Remove(walker);

            newWalkers.Clear();
            walkersToBeRemoved.Clear();
        }
    }

    static void PrintGrid()
    {
        for (var y = height - 1; y >= 0; y--)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write(grid[y, x]);
            }
            Console.WriteLine();
        }
    }

    static void PrintLongestPath()
    {
        foreach (var pos in lastWalker.Path)
        {
            grid[pos.Y, pos.X] = '0';
        }

        for (var y = height - 1; y >= 0; y--)
        {
            for (var x = 0; x < width; x++)
            {
                if (grid[y, x] == '0')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(grid[y, x]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(grid[y, x]);
                }
            }
            Console.WriteLine();
        }
    }

    static List<Point2d> CreatePathCopy(List<Point2d> path)
    {
        var copy = new List<Point2d>();
        foreach (var pos in path)
        {
            copy.Add(new(pos));
        }
        return copy;
    }

    static void ChangeDirection(this Walker walker)
    {
        if (grid[walker.Position.Y + 1, walker.Position.X] != '#' && !walker.Direction.Equals(down))
        {
            walker.Direction = up;
            return;
        }

        if (grid[walker.Position.Y - 1, walker.Position.X] != '#' && !walker.Direction.Equals(up))
        {
            walker.Direction = down;
            return;
        }

        if (grid[walker.Position.Y, walker.Position.X + 1] != '#' && !walker.Direction.Equals(left))
        {
            walker.Direction = right;
            return;
        }

        if (grid[walker.Position.Y, walker.Position.X - 1] != '#' && !walker.Direction.Equals(right))
        {
            walker.Direction = left;
            return;
        }
    }

    static Point2d OppositeDirectionOf(Point2d direction)
    {
        if (direction.Equals(up)) return down;
        if (direction.Equals(down)) return up;
        if (direction.Equals(left)) return right;
        return left;
    }

    static bool CanMove(this Walker walker)
    {
        return 
            grid[walker.Position.Y + walker.Direction.Y, walker.Position.X + walker.Direction.X] != '#';
    }

    static List<Point2d> GetSpawnDirections(Point2d walkerPosition, Point2d walkerDirection)
    {
        List<Point2d> directions = new();
        if (grid[walkerPosition.Y + 1, walkerPosition.X] == '.')
            directions.Add(up);

        if (grid[walkerPosition.Y - 1, walkerPosition.X] == '.')
            directions.Add(down);

        if (grid[walkerPosition.Y, walkerPosition.X + 1] == '.')
            directions.Add(right);

        if (grid[walkerPosition.Y, walkerPosition.X - 1] == '.')
            directions.Add(left);

        return directions;
    }

    static bool IsNode(this Point2d position)
    {
        int count = 0;

        if (grid[position.Y + 1, position.X] == '.')
            count++;
        if (grid[position.Y - 1, position.X] == '.')
            count++;
        if (grid[position.Y, position.X + 1] == '.')
            count++;
        if (grid[position.Y, position.X - 1] == '.')
            count++;

        return count > 2;
    }

    static void Init()
    {
        var input = File.ReadAllLines("Common/input23.txt");
        height = input.Length;
        width = input[0].Length;

        // create grid
        grid = new char[height, width];
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (input[height - y - 1][x] == '#')
                    grid[y, x] = '#';
                else
                    grid[y, x] = '.';
            }
        }

        // find start and end
        int i = 0;
        while (i < grid.GetLength(1))
        {
            if (grid[height - 1, i] == '.')
                start = new Point2d(i, height - 1);
            if (grid[0, i] == '.')
                end = new Point2d(i, 0);
            i++;
        }
    }
}
