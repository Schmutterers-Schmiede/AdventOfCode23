using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Day11
{
    public class Day11_Part2
    {
        private static char[,] space;
        private static List<Galaxy> galaxies;
        public static void Run()
        {
            Init();
            galaxies = GetGalaxies();
            ExpandSpace(1000000);
            var connections = Getconnections();
            Console.WriteLine($"sum of distances: {CalculateDistanceSum(connections)}");
        }

        private static long CalculateDistanceSum(List<(Galaxy g1, Galaxy g2)> connections)
        {            
            long distanceSum = 0;
            foreach (var connection in connections)
            {
                distanceSum += Math.Abs(connection.g1.X - connection.g2.X);
                distanceSum += Math.Abs(connection.g1.Y - connection.g2.Y);                
            }
            return distanceSum;
        }

        private static List<(Galaxy g1, Galaxy g2)> Getconnections()
        {
            List<(Galaxy g1, Galaxy g2)> connections = new List<(Galaxy g1, Galaxy g2)>();
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    connections.Add((galaxies[i], galaxies[j]));
                }
            }          
            return connections;
        }

        private static void ExpandSpace(int expansionDistance)
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
                    if (space[j, i] == '#') pathIsEmpty = false;
                }
                if (pathIsEmpty)
                    expandingColumns.Add(i);
            }
            
            //expand space rows
            for (int i = expandingRows.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < galaxies.Count; j++)
                {                    
                    if (galaxies[j].Y > expandingRows[i])
                    {
                        galaxies[j].Y += expansionDistance - 1;
                    }
                }
            }

            //expand space columns
            for (int i = expandingColumns.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < galaxies.Count; j++)
                {
                    if (galaxies[j].X > expandingColumns[i])
                    {
                        galaxies[j].X += expansionDistance - 1;
                    }
                }
            }
        }

        private static List<Galaxy> GetGalaxies()
        {
            List<Galaxy> galaxies = new List<Galaxy> ();
            for (int i = 0; i < space.GetLength (0); i++)
            {
                for (int j = 0; j < space.GetLength (1); j++)
                {
                    if (space[i,j] == '#')
                        galaxies.Add(new Galaxy(j,i));
                }
            }
            return galaxies;
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
            galaxies = new List<Galaxy>();
        }
    }
}
