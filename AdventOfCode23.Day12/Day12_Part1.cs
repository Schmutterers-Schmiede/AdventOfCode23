using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day12
{
    public class Day12_Part1
    {
        static List<string> records = new List<string>();
        

        public static void Run()
        {
            Init();
            int possibilities = 0;
            foreach (var record in records)
            {
                possibilities += LineResult(record);
            }
            Console.WriteLine($"number of possibilities: {possibilities}");
            
        }


        static int LineResult(string line)
        {            
            string[] substringBuffer = line.Split();
            char[] springRecord;

            int[] groupRecord = substringBuffer[1].Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            int possibilities = 0;
            string binaryString;

            int binaryIndex;
            int groupLengthCounter;                        
            int groupIndex;                      

            for (int i = GetNumberOfCombinationsForSpringRecord(substringBuffer[0]); i >= 0; i--)
            {
                springRecord = substringBuffer[0].ToCharArray();            
                binaryString = GetBinaryStringForInt(i, springRecord.Count(c => c == '?'));
                binaryIndex = 0;
                string[] groups;
                //fill in unknowns according to binary pattern
                for (int j = 0; j < springRecord.Length; j++)
                {
                    if (springRecord[j] == '?')
                    {
                        if (binaryString[binaryIndex] == '1')
                            springRecord[j] = '#';
                        else
                            springRecord[j] = '.';

                        binaryIndex++;
                    }
                }
                if (isValid(Regex.Replace( new string(springRecord).Trim('.'), "\\.{2,}", ".").Split('.'), groupRecord)) 
                    possibilities++;
                
            }

            Console.WriteLine($"{substringBuffer[0]} {possibilities}");
            return possibilities;
        }
        
        static bool isValid(string[] groups, int[] groupRecord)
        {
            if (groups.Length != groupRecord.Length) return false;

            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i].Length != groupRecord[i]) 
                    return false;
            }
            return true;
        }

        static string GetBinaryStringForInt(int value, int desiredLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Convert.ToString(value, 2));
            while(sb.Length < desiredLength)
            {
                sb.Insert(0, '0');
            }
            return sb.ToString();
        }

        static int GetNumberOfCombinationsForSpringRecord(string springRecord)
        {
            int length = 0;
            foreach (char c in  springRecord) 
            {
                if(c == '?')
                    length++;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append("1");
            }            
            return Convert.ToInt32(sb.ToString(), 2);
        }

        
        private static void Init()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day12/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());

            string[] substringBuffer = new string[2];            
            while(!sr.EndOfStream)
            {
                records.Add(sr.ReadLine());                
            }
        }
    }
}
