using System.Drawing;
using System.Text;

namespace AdventOfCode23.Day10
{
    public class Day10_Part1
    {

        private static char[,] grid;
        private static (int x, int y) start;
        public static void Run()
        {
            Init();            
            int steps = FindFurthestTile();
            Console.WriteLine($"distance to furthest tile: {Math.Ceiling(new decimal(steps/2))}");


        }

        private static int FindFurthestTile()
        {
            (int x, int y) pos = start;
            (int x, int y) previousPosition = (-1, -1);
            int steps = 0;

            do
            {
                var newPos = Step(pos, previousPosition);
                steps++;
                previousPosition = pos;
                pos = newPos;
            } while(pos != start);                        
            return steps;
        }

        private static (int x, int y) Step((int x, int y) pos, (int x, int y) previousPosition)
        {
            Console.WriteLine($"X {pos.x} Y{pos.y} {grid[pos.y, pos.x]}");
            var possibleDirections = DirectionsFor(pos);
            foreach (var direction in possibleDirections)
            {                
                if (    MoveIsValid(pos, direction) && 
                        previousPosition != (pos.x + direction.x, pos.y + direction.y))
                {
                    return (pos.x + direction.x, pos.y + direction.y);
                }
            }
            return (-1, -1); //error
        }

        private static bool MoveIsValid((int x, int y) pos, (int x, int y) direction)
        {
            var destination = (pos.x + direction.x, pos.y + direction.y);
            var possibleApproaches = DirectionsFor(destination);
            
            foreach(var approach in possibleApproaches)
            {
                if (direction.x + approach.x == 0 && direction.y + approach.y == 0)
                    return true;
            }            
            return false;

        }

        private static (int x, int y)[] DirectionsFor((int x, int y) pos)
        {
            switch(grid[pos.y, pos.x])
            {
                case '-':
                    return [(-1, 0), (1, 0)];   // left or right
                case '|':
                    return [(0, -1), (0, 1)];   // up or down
                case 'F':
                    return [(0, 1), (1, 0)];    // down or right
                case '7':
                    return [(0, 1), (-1, 0)];   // down or left
                case 'J':
                    return [(0, -1), (-1, 0)];  // up or left
                case 'L':
                    return [(0, -1), (1, 0)];  // up or right
                case '.':
                    return [];  // nowhere to go -> also no approaches
                default:
                    return [(0, 1), (0, -1), (1, 0), (-1, 0)]; // S -> anywhere
            }
        }

        private static void Init()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day10/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());

            string[] input = sr.ReadToEnd().Split("\r\n");
            grid = new char[input.Length, input[0].Length];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = input[i].Replace("\r", "");
                for (int j = 0; j < input[i].Length; j++)
                {
                    grid[i,j] = input[i][j];
                    if (grid[i, j] == 'S')
                        start = (j, i);
                }
            }
            
        }
    }
}
