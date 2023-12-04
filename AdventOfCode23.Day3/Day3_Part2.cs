using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day3
{
  public class Day3_Part2
  {
    private static char[][] grid;
    public static void Run()
    {
      StringBuilder path = new StringBuilder();
      path.Append(AppDomain.CurrentDomain.BaseDirectory);
      path.Append("../../../../AdventOfCode23.Day3/input2.txt");
      
      string[] lines = File.ReadAllLines(path.ToString());
      grid = new char[lines.Length][];
      for (int i = 0; i < lines.Length; i++)
      {
        grid[i] = lines[i].ToCharArray();
      }
      long sum = GearSum();
      Console.WriteLine();
      Console.WriteLine($"Sum of gear ratios: {sum}");

    }

    private static long GearSum()
    {
      long sum = 0;
      (int a, int b) neighbors;
      long result = 0;
      for (int i = 0; i < grid.Length; i++)
      {
        for (int j = 0; j < grid[i].Length; j++)
        {
          if (IsGear(j, i))
          {
            neighbors = GetNeighborsForGear(j, i);
            int product = neighbors.a * neighbors.b;
            sum += product;
            Console.WriteLine($"Gear at X {j,-3} Y {i,-3} : {neighbors.a,3} * {neighbors.b,3} = {product}");
          }
        }
      }
      return sum;
    }

    private static (int a, int b) GetNeighborsForGear(int x, int y)
    {
      int neighborCount = 0;
      List<int> neighbors = new List<int>();

      if (x > 0 && IsDigit(grid[y][x - 1])) neighbors.Add(ExtractNumber(x - 1, y)); // left
      if (x < grid[y].Length && IsDigit(grid[y][x + 1])) neighbors.Add(ExtractNumber(x + 1, y)); // right

      if (y > 0) //top row
      {
        if (IsDigit(grid[y - 1][x])) neighbors.Add(ExtractNumber(x, y - 1)); // top
        else
        {
          if (IsDigit(grid[y - 1][x - 1])) neighbors.Add(ExtractNumber(x - 1, y - 1)); // top right
          if (IsDigit(grid[y - 1][x + 1])) neighbors.Add(ExtractNumber(x + 1, y - 1)); // top right
        }
      }

      if (y < grid.Length) //bottom row
      {
        if (IsDigit(grid[y + 1][x])) neighbors.Add(ExtractNumber(x, y + 1)); // bottom
        else
        {
          if (IsDigit(grid[y + 1][x + 1])) neighbors.Add(ExtractNumber(x + 1, y + 1)); // bottom right
          if (IsDigit(grid[y + 1][x - 1])) neighbors.Add(ExtractNumber(x - 1, y + 1)); //bottom left                
        }
      }

      return (neighbors.First(), neighbors.Last());
    }

    private static int ExtractNumber(int x, int y)
    {
      int start = x;
      int end = x;
      while (start - 1 >= 0 && IsDigit(grid[y][start - 1]))
      {
        start--;
      }
      while (end + 1 < grid[y].Length && IsDigit(grid[y][end + 1]))
      {
        end++;
      }
      StringBuilder sb = new StringBuilder();
      for (int i = start; i <= end; i++)
      {
        sb.Append(grid[y][i]);
      }
      return int.Parse(sb.ToString());
    }

    private static bool IsStar(char c)
    {
      return c == '*';
    }

    private static bool IsGear(int x, int y)
    {
      if (!IsStar(grid[y][x])) return false;

      int neighborCount = 0;
      if (x > 0 && IsDigit(grid[y][x - 1])) neighborCount++; // left
      if (x < grid[y].Length && IsDigit(grid[y][x + 1])) neighborCount++; // right

      if (y > 0) //top row
      {
        if (IsDigit(grid[y - 1][x])) neighborCount++; // top
        else
        {
          if (IsDigit(grid[y - 1][x - 1])) neighborCount++; // top right
          if (IsDigit(grid[y - 1][x + 1])) neighborCount++; // top right
        }
      }

      if (y < grid.Length) //bottom row
      {
        if (IsDigit(grid[y + 1][x])) neighborCount++; // bottom
        else
        {
          if (IsDigit(grid[y + 1][x + 1])) neighborCount++; // bottom right
          if (IsDigit(grid[y + 1][x - 1])) neighborCount++; //bottom left
        }
      }
      return neighborCount == 2;
    }

    private static bool IsDigit(char c)
    {
      return c <= 57 && c >= 48;
    }
  }
}
