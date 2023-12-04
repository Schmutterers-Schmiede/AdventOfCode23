using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode23.Day1
{
    public class Day1_Part2
    {       
        public static void Run()
        {

            string line;
            long result = 0;
            int lineResult;
            int lineNr = 1;
            try
            {
                StringBuilder path = new StringBuilder();
                path.Append(AppDomain.CurrentDomain.BaseDirectory);
                path.Append("../../../../AdventOfCode23.Day1/input2.txt");
                StreamReader sr = new StreamReader(path.ToString());
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().ToLower();
                    lineResult = GetLineResult(line);
                    Console.Write($"line {lineNr,4}: {lineResult} ");
                    if (lineResult < 0) Console.Write(line);
                    Console.WriteLine();
                    lineNr++;
                    result += lineResult;
                }
                Console.WriteLine();
                Console.WriteLine($"sum is {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static bool LessThanXNumerals(string line, int x)
        {
            int result = 0;
            foreach (char c in line)
            {
                if (char.IsDigit(c)) result++;
            }
            return result < x;
        }



        private static int GetLineResult(string line)
        {
            int numeralIndex;

            numeralIndex = FindIndexOfFirstNumeral(line, 1);
            if (numeralIndex == -1) numeralIndex = line.Length;
            int digit1 = FindTextDigit(line.Substring(0, numeralIndex), 1);
            if (digit1 == -1)
                digit1 = Convert.ToInt32(line[numeralIndex] - 48);

            numeralIndex = FindIndexOfFirstNumeral(line, -1);
            if (numeralIndex == -1) numeralIndex = 0;
            int digit2 = FindTextDigit(line.Substring(numeralIndex), -1);
            if (digit2 == -1)
                digit2 = Convert.ToInt32(line[numeralIndex] - 48);

            int result = digit1 * 10 + digit2;
            return result;
        }

        private static int FindTextDigit(string segment, int direction)
        {
            if (direction != 1 && direction != -1) throw new ArgumentException("direction must be either 1 or -1");

            int result = -1;

            if (segment.Length < 3)
                return result;

            Func<int, int, bool> compare;
            if (direction == 1)
                compare = (int a, int b) => a < b;
            else
                compare = (int a, int b) => a > b;

            int startIndex = direction == 1 ? segment.Length : 0;
            int comp;

            Func<string, int> firstOrLastIndexOf;
            if (direction == 1) 
                firstOrLastIndexOf = (string pattern) => segment.IndexOf(pattern);
            else
                firstOrLastIndexOf = (string pattern) => segment.LastIndexOf(pattern);

            comp = firstOrLastIndexOf("one");
            if (comp != -1 && compare(comp, startIndex)) { result = 1; startIndex = comp; }
            comp = firstOrLastIndexOf("two");
            if (comp != -1 && compare(comp, startIndex)) { result = 2; startIndex = comp; }
            comp = firstOrLastIndexOf("three");
            if (comp != -1 && compare(comp, startIndex)) { result = 3; startIndex = comp; }
            comp = firstOrLastIndexOf("four");
            if (comp != -1 && compare(comp, startIndex)) { result = 4; startIndex = comp; }
            comp = firstOrLastIndexOf("five");
            if (comp != -1 && compare(comp, startIndex)) { result = 5; startIndex = comp; }
            comp = firstOrLastIndexOf("six");
            if (comp != -1 && compare(comp, startIndex)) { result = 6; startIndex = comp; }
            comp = firstOrLastIndexOf("seven");
            if (comp != -1 && compare(comp, startIndex)) { result = 7; startIndex = comp; }
            comp = firstOrLastIndexOf("eight");
            if (comp != -1 && compare(comp, startIndex)) { result = 8; startIndex = comp; }
            comp = firstOrLastIndexOf("nine");
            if (comp != -1 && compare(comp, startIndex)) { result = 9; }

            return result;
        }


        private static int FindIndexOfFirstNumeral(string line, int direction)
        {
            if (direction != 1 && direction != -1) throw new ArgumentException("direction must be either 1 or -1");

            int start = direction == 1 ? 0 : line.Length - 1;
            for (int i = start; i < line.Length && i >= 0; i += direction)
            {
                if (char.IsDigit(line[i]))
                    return i;
            }

            return -1;
        }
    }
}
