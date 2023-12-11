using System;
using System.Text;

namespace AdventOfCode23.Day11
{
    public class Day11_Part1
    {
        private static char[,] space;
        private static char[,] expandedSpace;
        private static void Run()
        {
            Init();
        }

        private static void ExpandSpace()
        {
            List<int> expandingRows = new List<int>();
            List<int> expandingColumns = new List<int>();

            bool pathIsEmpty;

            //get indices of empty rows
            for (int i = 0; i < space.GetLength(0); i++) 
            {
                pathIsEmpty = true;
                for (int j = 0; j < space.GetLength(1); j++)
                {
                    if (space[i, j] == '#') pathIsEmpty = false;
                }
                if (pathIsEmpty)
                    expandingRows.Add(i);
            }

            //get indices of empty columns
            for (int i = 0; i < space.GetLength(1); i++)
            {
                pathIsEmpty = true;
                for (int j = 0; j < space.GetLength(0); j++)
                {
                    if (space[j,i] == '#') pathIsEmpty = false;
                }
                if (pathIsEmpty)
                    expandingColumns.Add(i);
            }

            expandedSpace = new char[space.GetLength(0) + expandingRows.Count, space.GetLength(1) + expandingColumns.Count];

            //TODO
            int 
                iExpSpace = 0, 
                jExpSpace = 0;

            for(int i = 0; i <  space.GetLength(0); i++)
            {

                if (expandingRows.Contains(i))                    
                {
                    for (int j = 0; j < expandedSpace.GetLength(1); j++)
                    {
                        expandedSpace[iExpSpace, j] = '.';
                        expandedSpace[iExpSpace + 1, j] = '.';

                    }
                        iExpSpace += 2;
                }
                for(int j = 0; j < space.GetLength(1); j++)
                {
                   
                

                }
                
                iExpSpace++;
            }

            
           
        }
        private static void Init()
        {
            StringBuilder path = new StringBuilder();
            path.Append(AppDomain.CurrentDomain.BaseDirectory);
            path.Append("../../../../AdventOfCode23.Day11/input1.txt");
            StreamReader sr = new StreamReader(path.ToString());

            string[] input = sr.ReadToEnd().Split("\r\n");
            space = new char[input.GetLength(0), input.GetLength(1)];            

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = input[i].Replace("\r", "");
                for (int j = 0; j < input[i].Length; j++)
                {
                    space[i, j] = input[i][j];
                    
                }
            }
        }
    }
}
