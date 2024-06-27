using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode23.Day18.Part1
{
    public class Day18_Part1
    {
        static string[] instructions;
        static bool[,] grid;        
        static Point digger;
        static Point direction;
        static short steps;
        static List<Point> Q = new();
        static int area = 0;
        public static void Run()
        {
            Init();
            DigTrench();
            DigInterior();
            PrintLagoon();
            Console.WriteLine($"Total Area: {area}");
        }        

        static void DigTrench()
        {
            grid[digger.Y, digger.X] = true;
            foreach (var instruction in instructions)
            {                
                steps = short.Parse(Regex.Match(instruction, "\\b[0-9]{1,2}\\b").Value);
                switch (instruction[0])
                {
                    case 'U':
                        direction = new Point(0, 1);
                        break;
                    case 'D':
                        direction = new Point(0, -1);
                        break;
                    case 'L':
                        direction = new Point(-1, 0);
                        break;
                    case 'R':
                        direction = new Point(1, 0);
                        break;
                }
                for (int i = 0; i < steps; i++)
                {
                    Dig(direction);
                    area++;
                }
            }
        }

        static void DigInterior()
        {
            Point seed = new Point(0, 0);           
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] && !grid[y, x + 1])
                    {
                        seed = new Point(x + 1, y);
                        break;
                    }
                    if (grid[y, x] && grid[y, x + 1])
                    {                        
                        break;
                    }
                }
                if (seed.X != 0 && seed.Y != 0) break;
            }
            Q.Add(seed);
            Point current;
            while (Q.Count > 0)
            {
                current = Q.First();
                Q.RemoveAt(0);
                grid[current.Y, current.X] = true;
                area++;
                // up
                Look(current, new Point(0, 1));
                // down
                Look(current, new Point(0, -1));
                // right
                Look(current, new Point(1, 0));
                // left
                Look(current, new Point(-1, 0));
            }
        }

        static void Look(Point current, Point direction)
        {
            Point neighbor = new Point(current.X + direction.X, current.Y + direction.Y);
            if (!(grid[neighbor.Y, neighbor.X]) && !(Q.Contains(neighbor)))
                Q.Add(neighbor);
        }
        static void Dig(Point direction)
        {
            digger.X += direction.X;
            digger.Y += direction.Y;
            grid[digger.Y, digger.X] = true;

        }

        static void PrintLagoon()
        {
            for(int y = grid.GetLength(0) - 1; y >= 0; y--)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x])
                        Console.Write("X");
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        private static void Init()
        {
            instructions = File.ReadAllLines("Common/input.txt");
            var minX = 0;
            var minY = 0;
            var maxX = 0;
            var maxY = 0;
            var x = 0;
            var y = 0;
            int stepSize;
            foreach (var line in instructions)
            {
                stepSize = int.Parse(Regex.Match(line, "\\b[0-9]{1,2}\\b").Value);
                switch (line[0])
                {
                    case 'U':
                        y += stepSize;                        
                        break;
                    case 'D':
                        y -= stepSize;
                        break;
                    case 'L':
                        x -= stepSize;
                        break;
                    case 'R':
                        x += stepSize;
                        break;
                }
                if (y > maxY) maxY = y;
                if (y < minY) minY = y;
                if (x > maxX) maxX = x;
                if (x < minX) minX = x;
            }                   
            digger = new Point(Math.Abs(minX), Math.Abs(minY)); ;
            grid = new bool[Math.Abs(maxY - minY) + 1, Math.Abs(maxX - minX) + 1];
        }
    }
}
