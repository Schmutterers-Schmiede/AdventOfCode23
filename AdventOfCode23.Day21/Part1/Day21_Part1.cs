using System.Drawing;
using System.Net.WebSockets;

namespace AdventOfCode23.Day21;

public class Day21_Part1
{
    static char[,] grid;
    static Point start;
    public static void Run()
    {
        Init();
        var plots = BFS();
        PrintGrid();
        Console.WriteLine($"Reachable plots: {plots}");
    }

    static int BFS()
    {
        int plots = 0;
        HashSet<Point> visited = new();
        Queue<QEntry> unvisited = new();        
        QEntry current;               

        unvisited.Enqueue(new(start, 0));
        while (unvisited.Count > 0)        
        {
            current = unvisited.Dequeue();

            if (current.Steps % 2 == 0)
            {
                grid[current.Pos.Y, current.Pos.X] = 'O';
                plots++;
            }
            else { grid[current.Pos.Y, current.Pos.X] = 'o'; }

            Console.WriteLine($"processing ( X {current.Pos.X} | Y {current.Pos.Y} )");

            if (current.Steps != 65)
            {
                // look north
                if(CanGoTo(new(current.Pos.X, current.Pos.Y + 1)))
                    unvisited.Enqueue(new(new Point(current.Pos.X, current.Pos.Y + 1), current.Steps + 1));

                // look east
                if (CanGoTo(new(current.Pos.X + 1, current.Pos.Y)))
                    unvisited.Enqueue(new(new Point(current.Pos.X + 1, current.Pos.Y), current.Steps + 1));

                // look west
                if (CanGoTo(new(current.Pos.X - 1, current.Pos.Y)))
                    unvisited.Enqueue(new(new Point(current.Pos.X - 1, current.Pos.Y), current.Steps + 1));

                // look south
                if (CanGoTo(new(current.Pos.X, current.Pos.Y - 1)))
                    unvisited.Enqueue(new(new Point(current.Pos.X, current.Pos.Y - 1), current.Steps + 1));
            }
            visited.Add(current.Pos);
        }
        return plots;

        bool VisitedContains(Point position) => visited.Contains(position);
        bool UnvisitedContains(Point position) => unvisited.Any(x => x.Pos ==  position);
        static bool IsNotOutOfBounds(int x, int y) => x < grid.GetLength(1) && x >= 0 && y < grid.GetLength(0) && y >= 0;        
        bool CanGoTo(Point position) => !VisitedContains(position) && !UnvisitedContains(position) && IsNotOutOfBounds(position.X, position.Y) && grid[position.Y, position.X] == '.';
        
    }

    

    

    static void PrintGrid()
    {
        for (int y = grid.GetLength(0) - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Console.Write(grid[y,x]);
            }
            Console.WriteLine();
        }
    }

    static void Init()
    {
        var input = File.ReadAllLines("Common/input21.txt");
        grid = new char[input.Length, input[0].Length];
        
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                grid[y, x] = input[input.Length - y - 1][x];
                if (grid[y, x] == 'S')
                    start = new Point(x, y);
            }
        }        
        PrintGrid();
    }
}
