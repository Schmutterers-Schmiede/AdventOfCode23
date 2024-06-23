namespace AdventOfCode23.Day17
{

    /* GENERAL IDEA:
     * build graph from array
     * use an enum for directions
     * 
     * create heuristic in graph via pythagoras          
     * 
     * main loop: 
     *  - get next node from q
     *  
     *  - loop over edges
     *      - skip if 
     *          - edge goes backwards 
     *          - direction count is 4 and edge continues in same direction
     *          - node has already been processed
     *          - no edge in that direction
     *      - add to or update in q:
     *          - is this node in the q? (check with position)
     *              - if yes, update directionToReach, CostToReach and DirectionCount if necessary
     *              - if not, add it to q and sort q                 
     *  - add current node to path
     *  
     *  A* explaination: https://www.youtube.com/watch?v=ySN5Wnu88nE
     */
    public class Day17_Part1
    {
        static CityBlock[,] city;
        static CityBlock start;
        static CityBlock destination;
        static List<QEntry> unvisitedQ = new();
        static Dictionary<(int, int), PathEntry> pathTable = new();

        public static void Run()
        {
            Init();
            AStar();
            var heatloss = GetOverallHeatLoss();
            PrintPath();
            VisualizePath();
            Console.WriteLine($"Overall heat loss: {heatloss}");
        }

        public static void VisualizePath()
        {            
            Console.WriteLine();
            char[,] map = new char[city.GetLength(0), city.GetLength(1)];
            for (int y = 0; y < city.GetLength(0); y++)
            {
                for (int x = 0; x < city.GetLength(1); x++)
                {
                    map[y, x] = Convert.ToChar(city[y, x].HeatLoss + '0');
                }
            }
            var current = pathTable[destination.Position];            
            do
            {
                map[current.Block.Position.Y, current.Block.Position.X] = 'X';
                current = current.Previous;
            } while (!current.Block.Equals(start));

            for (int y = 0;y < map.GetLength(0); y++)
            {
                for(int x = 0;x < map.GetLength(1); x++)
                {
                    if (map[y,x] == 'X')
                        Console.ForegroundColor = ConsoleColor.Green;

                    Console.Write(map[y,x]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }

        static void PrintPath()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Path:");
            var current = pathTable[destination.Position];
            while (current != null)
            {
                Console.WriteLine($"(X {current.Block.Position.X} | Y {current.Block.Position.Y} ) - {current.Block.HeatLoss}");                
                current = current.Previous;
            }            
        }
        static int GetOverallHeatLoss()
        {            
            var current = pathTable[destination.Position];
            int heatLoss = 0;
            while (current != null)
            {                         
                heatLoss += current.Block.HeatLoss;
                current = current.Previous;
            }
            return heatLoss;
        }

        static void AStar()
        {
            
            

            QEntry? currentEntry = null;
            CityBlock edgeTarget = null;

            do
            {
                currentEntry = GetNextInQ();

                PrintWithIndent($"Processing ( X {currentEntry.Block.Position.X} | Y {currentEntry.Block.Position.Y} ):", 0);

                for (Directions d = Directions.Up; d <= Directions.Right; d++)
                {                    

                    // skip if going backwards
                    if (d == OppositeDirectionOf(currentEntry.DirectionToReach))
                    {                        
                        continue;
                    }
                    // skip if too many moves in same direction
                    if (d == currentEntry.DirectionToReach &&
                        currentEntry.DirectionCount == 4)
                    {                        
                        continue;
                    }
                    // skip if no edge in this direction
                    if (!currentEntry.Block.Edges.ContainsKey(d))
                        continue;

                    edgeTarget = currentEntry.Block.Edges[d].Target;

                    // skip if node has already been processed
                    if (pathTable.ContainsKey(edgeTarget.Position))
                        continue;

                    PrintWithIndent($"Processing Edge {d}", 1);

                    // neighboring node already in q -> try update
                    if (QContains(edgeTarget))
                    {
                        PrintWithIndent($"Node already in q -> update", 2);
                        var costToReachEdgeTarget = currentEntry.CostToReach + edgeTarget.HeatLoss;
                        if (costToReachEdgeTarget < GetQEntryByPosition(edgeTarget.Position).CostToReach)
                        {
                            UpdateQEntry(edgeTarget, costToReachEdgeTarget, d, currentEntry.DirectionCount + 1);
                        }
                    }
                    // neighboring node not in q -> add
                    else
                    {
                        PrintWithIndent($"Node not in q -> add", 2);
                        var directionCount = d == currentEntry.DirectionToReach ? currentEntry.DirectionCount + 1 : 1;
                        AddToQ(new(
                            edgeTarget,
                            currentEntry.Block,
                            d,
                            currentEntry.CostToReach + edgeTarget.HeatLoss,
                            directionCount));
                    }
                }
                if (currentEntry.Block.Equals(start))
                {
                    pathTable.Add(
                        currentEntry.Block.Position,
                        new(
                            currentEntry.Block,
                            null));
                }
                else
                {
                    pathTable.Add(
                        currentEntry.Block.Position,
                        new(
                            currentEntry.Block,
                            pathTable[currentEntry.ReachedFrom.Position]));                    
                }
            } 
            while (!currentEntry.Block.Equals(destination));            
        }

        
        static Directions OppositeDirectionOf(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:      return Directions.Down;
                case Directions.Left:    return Directions.Right;
                case Directions.Down:    return Directions.Up;
                case Directions.Right:   return Directions.Left;
                default:                return Directions.Nowhere;
            }
        }

        private static void UpdateQEntry(
    CityBlock b,
    int costToReach,
    Directions directionToReach,
    int directionCount)
        {
            var entryToUpdate = unvisitedQ.Where((QEntry e) => e.Block.Equals(b)).First();
            entryToUpdate.CostToReach = costToReach;
            entryToUpdate.DirectionCount = directionCount;
            entryToUpdate.DirectionToReach = directionToReach;
        }

        private static QEntry GetNextInQ()
        {
            var next = unvisitedQ.First();
            unvisitedQ.Remove(next);
            return next;
        }

        private static void SortQ()
        {
            unvisitedQ = unvisitedQ.OrderBy((QEntry e) => e.CombinedCost()).ToList();
        }

        private static bool QContains(CityBlock c)
        {
            return unvisitedQ.Where((QEntry x) => x.Block.Equals(c)).Count() == 1;
        }

        private static QEntry GetQEntryByPosition((int x, int y) pos)
        {
            return unvisitedQ.Where((QEntry e) => e.Block.Position == pos).First();
        }

        private static void AddToQ(QEntry e)
        {
            unvisitedQ.Add(e);
            SortQ();
        }

        static void Init()
        {
            var input = File.ReadAllLines("./input17.txt");            
            city = new CityBlock[input.Length, input[0].Length];            
            //initialize nodes without edges
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    city[y,x] = new CityBlock(Convert.ToInt32(input[y][x]) - '0', HeuristicValue(city.GetLength(1) - x, city.GetLength(0) - y) , x, y);
                }
            }

            start = city[0, 0];            
            destination = city[city.GetLength(0) - 1, city.GetLength(1) - 1]; 

            //fill in edges
            for (int y = 0;y < city.GetLength(0); y++) 
            {
                for (int x = 0;x < city.GetLength(1); x++)
                {
                    // connect top
                    if (y > 0) 
                        city[y, x].Edges[Directions.Up] = new Edge(city[y-1,x]);
                    // connect left
                    if(x > 0)
                        city[y, x].Edges[Directions.Left] = new Edge(city[y,x-1]);
                    // connect bottom
                    if(y < city.GetLength(0)-1)
                        city[y, x].Edges[Directions.Down] = new Edge(city[y+1,x]);
                    // connect right
                    if(x < city.GetLength(1)-1)
                        city[y, x].Edges[Directions.Right] = new Edge(city[y,x+1]);
                }
            }

            //direction to reach does not matter at start
            AddToQ(                
                new (
                block: start,
                reachedFrom: null,
                directionToReach: Directions.Nowhere,
                costToReach: 2,
                directionCount: 0)
                );            
        }

        private static double HeuristicValue(int x, int y) 
        {
            int xDistance = city.GetLength(1) - x;
            int yDistance = city.GetLength(0) - y;
            return xDistance + (yDistance * 2);
            //return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }       

        private static void PrintWithIndent(string message, int indentLevel)
        {
            for (int i = 0; i < indentLevel; i++)
            {
                Console.Write("  ");
            }
            Console.WriteLine(message);
        }
    }

}
