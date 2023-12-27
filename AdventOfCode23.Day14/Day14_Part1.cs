namespace AdventOfCode23.Day14
{
    public class Day14_Part1
    {
        static char[,] platform;
        public static void Run()
        {
            Init();
            //PrintPlatform();
            Tilt();
            //PrintPlatform();
            int load = CalculateLoad();
            Console.WriteLine($"{load}");

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

        static void Tilt()
        {
            for (int row = 1; row < platform.GetLength(0);  row++)
            {
                for (int col = 0; col < platform.GetLength(1); col++)
                {
                    if (platform[row, col] == 'O')
                    {
                        Roll(row, col);
                    }
                }
            }
        }

        static void Roll(int row, int col)
        {
            int distance = 0;
            while (row - distance - 1 >= 0 && platform[row - distance - 1, col] == '.')
            {
                distance++;
            }
            if(distance > 0)            
            Swap(row, col, row - distance, col);
            
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
            
        }
    }
}
