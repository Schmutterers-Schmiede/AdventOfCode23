using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode23.Day13
{
    public class Day13_Part1
    {
        static List<char[,]> fields = new List<char[,]>();
        public static void Run()
        {
            Init();
            int sum = 0;
            foreach (var field in fields)
            {
                sum += FieldResult(field);
            }
            Console.WriteLine($"{sum}");
        }

        static int FieldResult(char[,] field)
        {
            int resh = HorizontalMirrorResult(field);
            int resv = VerticalMirrorResult(field);
            Console.WriteLine($"{resh} {resv}");
            return resv + resh;
        }

        static int HorizontalMirrorResult(char[,] field)
        {
            bool possibleMirrorFound;
            int mirrorIndex = -1;
            for (int i = 0; i < field.GetLength(0) - 1; i++)
            {
                possibleMirrorFound = true;
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] != field[i + 1, j])
                    {
                        possibleMirrorFound = false; 
                        break;
                    }
                }
                if (possibleMirrorFound)
                {
                    if(IsHorizontalMirror(field, i))
                    {
                        mirrorIndex = i;
                        break;                    
                    }
                } 
            }

            if (mirrorIndex == -1)
                return 0;
            
            
            
            return (mirrorIndex + 1) * 100;
        }

        static bool IsHorizontalMirror(char[,] field, int mirrorIndex) 
        {
            int up = mirrorIndex; int down = mirrorIndex + 1;
            while (up >= 0 && down < field.GetLength(0))
            {
                for (int i = 0; i < field.GetLength(1); i++)
                {
                    if (field[up, i] != field[down, i])
                        return false;
                }
                up--;
                down++;
            }
            return true;
        }
            
        static int VerticalMirrorResult(char[,] field)
        {
            bool mirrorFound;
            int mirrorIndex = -1;
            for (int i = 0; i < field.GetLength(1) - 1; i++)
            {
                mirrorFound = true;   
                for (int j = 0; j < field.GetLength(0); j++)
                {
                    if (field[j,i] != field[j,i + 1])
                    {
                        mirrorFound = false; 
                        break;
                    }
                }
                if (mirrorFound)
                {
                    mirrorIndex = i;
                    break;
                }

            }

            if (mirrorIndex == -1)
                return 0;
            
            
            return mirrorIndex +1;
        }
        
        static bool IsVerticalMirror(char[,] field, int mirrorIndex) 
        {
            int left = mirrorIndex; int right = mirrorIndex + 1;
            while (left >= 0 && right < field.GetLength(1))
            {
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    if (field[i, left] != field[i, right])
                        return false;
                }
                left--;
                right++;
            }
            return true;
        }

        public static void Init()
        {
            var input = File.ReadAllText("input1.txt");
            var fieldParagraphs = input.Split("\r\n\r\n");
            string[] fieldLines;
            foreach (var field in fieldParagraphs)
            {
                fieldLines = field.Split("\r\n");
                char[,] fieldArray = new char[fieldLines.Length, fieldLines[0].Length];

                for (int i = 0; i < fieldLines.Length; i++)
                {
                    for (int j = 0; j < fieldLines[i].Length; j++)
                    {
                        fieldArray[i,j] = fieldLines[i][j];
                    }
                }
                fields.Add(fieldArray);
            }            
        }
    }
}
