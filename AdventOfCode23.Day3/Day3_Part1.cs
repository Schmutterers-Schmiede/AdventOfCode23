using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode23.Day3
{
    public class Day3_Part1
    {
        private static char[][] grid;
        public static void Run()
        {            
            string[] lines = File.ReadAllLines("F:\\Coding\\adventOfCode\\AdventOfCode23\\AdventOfCode23.Day3\\input1.txt");
            grid = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                grid[i] = lines[i].ToCharArray();
            }

            int sum = PartSum();
            Console.WriteLine();
            Console.WriteLine($"Sum of parts: {sum}");

        }

        private static int PartSum()
        {
            
            int sum = 0;
            int num;
            int end;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (IsDigit(grid[i][j]))
                    {
                        end = FindEndOfNumber(j, i);
                        if (BordersOnSymbol(j, i, end))
                        {
                            num = getNumberFromGrid(j, i, end);
                            Console.WriteLine($"line {i}: {num}");
                            sum += num;
                        }
                        j = end;
                    }                    
                }
            }
            return sum;
        }

        private static bool IsDigit(char c)
        {
            return c <= 57 && c >= 48;
        }

        private static bool IsDot(char c)
        {
            return c == 46;                                                 
        }

        private static bool IsSymbol(char c)
        {
            return !IsDigit(c) && !IsDot(c);
        }

        private static int FindEndOfNumber(int x, int y)
        {
            int i = x;
            while (i + 1 < grid[y].Length && IsDigit(grid[y][i + 1]))
            {
                i++;                
            }
            return i;
        }

        private static int getNumberFromGrid(int x, int y, int end)
        {           
            StringBuilder sb = new StringBuilder();            
            for (int i = x; i <= end; i++)
            {
                sb.Append(grid[y][i]);
            }
            return int.Parse(sb.ToString());
        }

        /// <summary>
        /// checks if a number with start coordinates x and y borders on a symbol
        /// </summary>
        /// <param name="x">x coord of start of number</param>
        /// <param name="y">y coord of start of number</param>
        /// <returns></returns>
        private static bool BordersOnSymbol(int x , int y, int end)
        {            
            if (CheckSide(x, y, -1)) return true;
            if(CheckTopBottom(x-1, y)) return true;
            
            for (int i = x; i <= end; i++)
            {
                if(CheckTopBottom(i, y)) return true;
            }

            if (CheckSide(end, y, 1)) return true;
            if (CheckTopBottom(end + 1, y)) return true;

            return false;
        }

        private static bool CheckSide(int x, int y, int direction)
        {
            int xIndex = x + direction;
            if(xIndex < grid[y].Length && xIndex >= 0) return IsSymbol(grid[y][xIndex]);
            return false;
        }

        private static bool CheckTopBottom(int x, int y)
        {
            if(x < grid[y].Length && x >= 0)
            {
                
                if(y > 0)
                {
                    
                    if (IsSymbol(grid[y-1][x]))
                        return true;
                }
                if(y < grid.Length - 1)
                {
                    
                    if (IsSymbol(grid[y+1][x]))
                        return true;
                }
            }
            return false;
        }
    }
}
