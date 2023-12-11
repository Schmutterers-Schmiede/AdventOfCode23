using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode23.Day11
{
    public class Day11_Part1
    {
        private static char[,] space;
        private static char[,] expandedSpace;        
        public static void Run()
        {
            Init();
            PrintCharArray(space);
            ExpandSpace();
            PrintCharArray(expandedSpace);
            var connections = Getconnections();
            Console.WriteLine($"sum of distances: {CalculateDistanceSum(connections)}");


        }

        private static int CalculateDistanceSum(List<((int x, int y) galaxy1, (int x, int y) galaxy2)> connections)
        {
            int ySteps, xSteps;
            int distanceSum = 0;
            foreach (var connection in connections)
            {
                xSteps = Math.Max(connection.galaxy1.x, connection.galaxy2.x) - Math.Min(connection.galaxy1.x, connection.galaxy2.x);
                ySteps = Math.Max(connection.galaxy1.y, connection.galaxy2.y) - Math.Min(connection.galaxy1.y, connection.galaxy2.y);
                distanceSum += xSteps + ySteps;
            }
            return distanceSum;
        }

        private static List<((int x, int y) galaxy1, (int x, int y) galaxy2)> Getconnections()
        {
            List<(int x, int y)> galaxies = new List<(int x, int y)>();
            for (int i = 0; i < expandedSpace.GetLength(0); i++)
            {
                for (int j = 0; j < expandedSpace.GetLength(1); j++)
                {
                    if (expandedSpace[i, j] == '#')
                    {
                        galaxies.Add((j, i));
                    }
                }
            }

            List<((int x, int y) galaxy1, (int x, int y) galaxy2)> connections = new List<((int x, int y) galaxy1, (int x, int y) galaxy2)>();
            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for(int j = i + 1;j < galaxies.Count; j++)
                {
                    connections.Add((galaxies[i], galaxies[j]));
                }
            }
            return connections;
        }

        private static void PrintCharArray(char[,] array) 
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for(int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
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
            
            //populate expanded space
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
                    iExpSpace ++;
                }
                else
                {
                    jExpSpace = 0;
                    for(int j = 0; j < space.GetLength(1); j++)
                    {
                        if (expandingColumns.Contains(j))
                        {
                            expandedSpace[iExpSpace, jExpSpace] = space[i, j];
                            jExpSpace ++;
                            expandedSpace[iExpSpace, jExpSpace] = space[i,j];
                        }
                        else
                        {
                            expandedSpace[iExpSpace, jExpSpace] = space[i, j];
                        }
                        jExpSpace++;
                    }
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
            space = new char[input.Length, input[0].Length];            

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
