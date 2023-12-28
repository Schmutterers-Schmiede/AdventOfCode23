using System.Runtime.CompilerServices;

namespace AdventOfCode23.Day16
{
    public class Day16_Part2
    {
        static char[,] grid; // input as char array, will not be altered
        static bool[,] energizedFields; // array for keeping track of energized positions
        static List<Beam> activeBeams = new List<Beam>(); 

        //acitiveBeams cannot be changed during iteration, so all changes must be saved for the end of each step
        static List<Beam> beamsForDeletion = new List<Beam>(); 
        static List<Beam> newBeams = new List<Beam>();

        // splitters can cause cycles -> disable after first usage since triggering them multiple times won't make any difference
        static List<(int, int)> spentSplitterPositions = new List<(int, int)>(); 

        static List<int> simulationResults = new List<int>();// save all results in order to pick the highest in the end
        
        public static void Run()
        {
            Init();
            //start left and right of the grid
            for(int i = 0; i < grid.GetLength(0); i++)
            {
                simulationResults.Add(RunSimulation(0, i, 1, 0));
                simulationResults.Add(RunSimulation(grid.GetLength(1) - 1, i, -1, 0));
            }

            //start at top and bottom of the grid
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                simulationResults.Add(RunSimulation(i, 0, 0, 1));
                simulationResults.Add(RunSimulation(i, grid.GetLength(0) - 1, 0, -1));
            }

            Console.WriteLine($"Highest sunlight utilization: {simulationResults.Max()}");
        }
        static int RunSimulation(int startX, int startY, int xDirection, int yDirection)
        {
            placeInitialBeam(startX, startY, xDirection, yDirection);   
            while (!(activeBeams.Count == 0))
            {
                foreach(var beam in activeBeams)                
                    Step(beam);               

                //remove or add active beams if necessary
                foreach(var beam in beamsForDeletion)                
                    activeBeams.Remove(beam);                

                foreach (var beam in newBeams)                
                    activeBeams.Add(beam);      
                                
                beamsForDeletion.Clear();
                newBeams.Clear();
            }
            //reset for next simulation
            activeBeams.Clear();
            spentSplitterPositions.Clear();

            //get result and reset energized fields array
            int result = CountEnerGizedFields();
            for(int i = 0; i < energizedFields.GetLength(0); i++)
            {
                for(int j = 0; j < energizedFields.GetLength(1); j++)
                {
                    energizedFields[i, j] = false;
                }
            }
            return result;
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
        
        //process the character on the current position of the current beam
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
            int mirrorOrientationFactor; //for flipping direction values
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
                //always set energized field as soon as it is definitely going to be energized (also done in TrySpawn)
                energizedFields[newY, newX] = true;
            }
        }

        static void placeInitialBeam(int x, int y, int xDirection, int yDirection)
        {
            activeBeams.Add(new Beam(x, y, xDirection, yDirection));
            energizedFields[y,x] = true;
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
        }
    }
}
