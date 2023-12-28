using System.Runtime.CompilerServices;

namespace AdventOfCode23.Day16
{
    public class Day16_Part1
    {
        static char[,] grid;
        static bool[,] energizedFields;
        static List<Beam> activeBeams = new List<Beam>();
        static List<Beam> beamsForDeletion = new List<Beam>();
        static List<Beam> newBeams = new List<Beam>();
        static List<(int, int)> spentSplitterPositions = new List<(int, int)>(); // splitters can cause cycles -> disable after usage
        public static void Run()
        {
            Init();
            while (!(activeBeams.Count == 0))
            {
                foreach(var beam in activeBeams)                
                    Step(beam);               

                foreach(var beam in beamsForDeletion)                
                    activeBeams.Remove(beam);                

                foreach (var beam in newBeams)                
                    activeBeams.Add(beam);      
                
                beamsForDeletion.Clear();
                newBeams.Clear();
            }

            PrintEnergizedFields();
            Console.WriteLine();
            Console.WriteLine($"number of energized fields: {CountEnerGizedFields()}");
        }

        static void PrintEnergizedFields()
        {
            for(int i = 0; i < energizedFields.GetLength(0); i++)
            {
                for(int j  = 0; j <  energizedFields.GetLength(1); j++)
                {
                    if (energizedFields[i, j]) Console.Write('#');
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }
        static int CountEnerGizedFields()
        {
            int result = 0;
            for(int i = 0; i < energizedFields.GetLength(0); i++)
            {
                for (int j = 0; j < energizedFields.GetLength(1); j++)
                {
                    if (energizedFields[i, j]) result++;
                }
            }
            return result;
        }
        
        static void Step(Beam beam)
        {
            switch(grid[beam.PosY, beam.PosX])
            {
                case '.':
                    moveBeam(beam);                
                    break;
                case '-':
                    if (spentSplitterPositions.Contains((beam.PosX, beam.PosY)))
                    {
                        beamsForDeletion.Add(beam);
                        break;
                    }

                    if (beam.YDirection != 0)
                    {
                        spentSplitterPositions.Add((beam.PosX, beam.PosY));
                        TrySpawn(beam.PosX + 1, beam.PosY, 1, 0);
                        beam.YDirection = 0;
                        beam.XDirection = -1;
                        moveBeam(beam);
                    }
                    else
                        moveBeam(beam);

                    
                    break;

                case '|':
                    if(spentSplitterPositions.Contains((beam.PosX, beam.PosY)))
                    {
                        beamsForDeletion.Add(beam);
                        break;
                    }

                    if (beam.XDirection != 0)
                    {
                        spentSplitterPositions.Add((beam.PosX, beam.PosY));
                        TrySpawn(beam.PosX, beam.PosY + 1, 0, 1);
                        beam.YDirection = -1;
                        beam.XDirection = 0;
                        moveBeam(beam);
                    }
                    else 
                        moveBeam(beam);

                    break;

                case '/':
                    Reflect(beam, '/');
                    break;

                case '\\':
                    Reflect(beam, '\\');
                    break;
                
            }
        }

        static void Reflect(Beam beam, char mirror)
        {
            int mirrorOrientationFactor;
            if (mirror == '/') mirrorOrientationFactor = -1;
            else mirrorOrientationFactor = 1;

            int buffer = beam.XDirection * mirrorOrientationFactor;
            beam.XDirection = beam.YDirection * mirrorOrientationFactor;
            beam.YDirection = buffer;

            moveBeam(beam);
        }

        static bool IsOutOfBounds(int x, int y)
        {
            return  x < 0 ||
                    x >= grid.GetLength(1) ||
                    y < 0 ||
                    y >= grid.GetLength(0);
        }

        static void TrySpawn(int x, int y, int xDirection, int yDirection)
        {
            if(!IsOutOfBounds(x, y))
            {
                newBeams.Add(new Beam(x, y, xDirection, yDirection));
                energizedFields[y,x] = true;
            }
        }
        
        static void moveBeam(Beam beam)
        {
            int newX = beam.PosX + beam.XDirection;
            int newY = beam.PosY + beam.YDirection;
           
            if (IsOutOfBounds(newX, newY))
                beamsForDeletion.Add(beam);

            else
            {
                beam.PosY = newY;
                beam.PosX = newX;
                energizedFields[newY, newX] = true;
            }
        }

        static void Init()
        {
            var input = File.ReadAllLines("input.txt");
            grid = new char[input.Length, input[0].Length];
            energizedFields = new bool[input.Length, input[0].Length];
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    grid[i,j] = input[i][j];
                }
            }
            activeBeams.Add(new Beam(0, 0, 1, 0));
            energizedFields[0,0] = true;    
        }
    }
}
