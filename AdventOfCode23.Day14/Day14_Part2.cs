using System.Collections.Concurrent;
using System.Text;

namespace AdventOfCode23.Day14
{
    public class Day14_Part2
    {
        static char[,] platform;
        static char[,] originalPlatform;
        static List<int> knownLoadsInOrder = new List<int>();
        
        
        public static void Run()
        {
            

            Init();
            int load;
            int i = 0;
            int patternStartIndex;
            int patternEndIndex;

            while (i < 300)
            {
                //PrintPlatform();

                TiltNorthOrWest(1);    // north
                //PrintPlatform();

                TiltNorthOrWest(0);    // west
                //PrintPlatform();

                TiltSouthOrEast(1);     // south
                //PrintPlatform();

                TiltSouthOrEast(0);     // east
                //PrintPlatform();

                load = CalculateLoad();

                knownLoadsInOrder.Add(load);
                

                
                Console.WriteLine($"cycle {i + 1}: {load}");
                
                i++;
            }
            int finalLoad = CalculateFinalLoad();
            Console.WriteLine();
            Console.WriteLine($"final load: {finalLoad}");

        }
        
        static int CalculateFinalLoad()
        {
           
            Dictionary<int, int> knownLoads = new Dictionary<int, int>();
            for (int i = 0; i < knownLoadsInOrder.Count; i++)
            {
                if (knownLoads.ContainsKey(knownLoadsInOrder[i]))
                {
                    int startIndex = knownLoads[knownLoadsInOrder[i]];
                    int endIndex = i;
                    int spanLength = endIndex - startIndex;
                    bool patternFound = true;
                    for (int j = startIndex; j <= endIndex; j++)
                    {
                        if (knownLoadsInOrder[j] != knownLoadsInOrder[j + spanLength])
                        {
                            patternFound = false; break;
                        }
                    }
                    if (patternFound)
                    {
                        return knownLoadsInOrder[startIndex + ((1000000000 - startIndex) % spanLength) - 1];
                    }
                    knownLoads[knownLoadsInOrder[i]] = i;
                }
                else
                    knownLoads.Add(knownLoadsInOrder[i], i);
            }
            return 0;
        }
        static void PrintPlatform()
        {
            for(int row = 0; row < platform.GetLength(0); row++)
            {
                for(int col = 0; col < platform.GetLength(1); col++)
                {
                    Console.Write(platform[row,col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static int CalculateLoad()
        {
            int sum = 0;
            for (int row = 0; row < platform.GetLength(0); row++)
            {
                for(int col = 0; col < platform.GetLength(1); col++)
                {
                    if (platform[row, col] == 'O')
                        sum += platform.GetLength(0) - row;
                }
            }
            return sum;
        }

        // directions: 1 for north, 0 for west
        static void TiltNorthOrWest(int direction)
        {
            for (int row = 0; row < platform.GetLength(0);  row++)
            {
                for (int col = 0; col < platform.GetLength(1); col++)
                {
                    if (platform[row, col] == 'O')
                    {
                        if(direction == 1)
                            RollNorth(row, col);
                        else    
                            RollWest(row, col);
                    }
                }
            }
        }

        //directions: 1 for south, 0 for east
        static void TiltSouthOrEast(int direction)
        {
            for (int row = platform.GetLength(0) - 1; row >= 0; row--)
            {
                for (int col = platform.GetLength(1) - 1; col >= 0; col--)
                {
                    if (platform[row, col] == 'O')
                    {
                        if (direction == 1)
                            RollSouth(row, col);
                        else
                            RollEast(row, col);
                    }
                }
            }
        }

        static void RollNorth(int row, int col)
        {
            int distance = 0;
            
            while (row - distance - 1 >= 0 && platform[row - distance - 1, col] == '.')
            {
                distance++;
            }
            if (distance > 0)
                Swap(row, col, row - distance, col);
        }


        static void RollSouth(int row, int col)
        {
            int distance = 0;            
            while (row + distance + 1 < platform.GetLength(0) && platform[row + distance + 1, col] == '.')
            {
                distance++;
            }
            if(distance > 0)            
            Swap(row, col, row + distance, col);           
        }

        static void RollWest(int row, int col)
        {
            int distance = 0;
            while(col - distance - 1 >= 0 && platform[row, col - distance - 1] == '.') 
            {
                distance++;
            }
            if (distance > 0)
                Swap(row, col - distance, row, col);
        }

        static void RollEast(int row, int col)
        {
            int distance = 0;   
            while(col + distance + 1 < platform.GetLength(1) && platform[row, col + distance + 1] == '.')
            {
                distance++;
            }
            if (distance > 0) 
                Swap(row, col, row, col + distance);
        }

        static void Swap(int row1, int col1, int row2, int col2)
        {
            char buffer = platform[row1, col1];
            platform[row1, col1] = platform[row2, col2];
            platform[row2, col2] = buffer;
        }

     

        static void Init()
        {
            var input = File.ReadAllLines("input.txt");
            platform = new char[input.Length, input[0].Length];
            for(int i  = 0; i < input.Length; i++) 
            { 
                for(int j = 0; j < input[i].Length; j++)
                {
                    platform[i,j] = input[i][j];
                }
            }
            originalPlatform = new char[platform.GetLength(0), platform.GetLength(1)];
            Array.Copy(platform, originalPlatform, platform.Length);
        }
    }
}
