using System.Text;

namespace AdventOfCode23.Day1
{
  public static class Day1_Part1
  {
    public static void Run()
    {
      string line;
      int result = 0;
      int digit1 = 0;
      int digit2 = 0;
      int lineNr = 1;
      try
      {
        StringBuilder path = new StringBuilder();
        path.Append(AppDomain.CurrentDomain.BaseDirectory);
        path.Append("../../../../AdventOfCode23.Day1/input1.txt");
        StreamReader sr = new StreamReader(path.ToString());
        while (!sr.EndOfStream)
        {
          line = sr.ReadLine();
          foreach (char c in line)
          {
            if (char.IsDigit(c))
            {
              digit1 = Convert.ToInt32(c) - 48;
              break;
            }
          }
          foreach (char c in line.Reverse())
          {
            if (char.IsDigit(c))
            {
              digit2 = Convert.ToInt32(c) - 48;
              break;
            }
          }
          Console.WriteLine($"line {lineNr}: {digit1}{digit2}");
          lineNr++;
          result += digit1 * 10 + digit2;
        }
        Console.WriteLine($"sum is {result}");
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}
