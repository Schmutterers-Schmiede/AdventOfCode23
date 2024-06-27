using System.Text;
using AdventOfCode23.Day17.Common;

namespace AdventOfCode23.Day17
{

    /* GENERAL IDEA A*:
     * build graph from input
     * use an enum for directions
     * 
     * calculate heuristic value of each node using pythagorean theorem
     * 
     * main loop: 
     *  - get next node from q
     *  
     *  - loop over edges
     *      - skip if 
     *          - edge goes backwards 
     *          - direction count is 3 and edge continues in same direction
     *          - node has already been processed
     *          - no edge in that direction
     *      - add to or update in q:
     *          - is this node in the q? (check with id)
     *              - if yes, update directionToReach, CostToReach and DirectionCount if necessary
     *              - if not, add it to q
     *  - add current node to path
     *  
     *  A* explaination: https://www.youtube.com/watch?v=ySN5Wnu88nE
     *  
     *  
     *  create a class BlockNode that is used for both q and path with a property id and idOfPrevious
     *  calculate unique id from path key data and 
     */
    public class Day17_Part1
    {
        // settings        
        static int directionalLimit = 3;

        static CityBlock[,] city;
        static CityBlock start;
        static CityBlock destination;

        static List<BlockEntry> Q = new();
        static Dictionary<string, BlockEntry> pathGraph = new();
        static string destinationId;


        public static void Run()
        {
            Init();
            AStar();
            var heatloss = GetOverallHeatLoss();
            PrintPath();
            VisualizePath();

            Console.WriteLine();
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

            var current = pathGraph[destinationId];

            map[0, 0] = 'X';
            while (!current.Block.Equals(start))
            {
                switch (current.DirectionToReach)
                {
                    case Directions.Up:
                        map[current.Block.Position.Y, current.Block.Position.X] = '^';
                        break;
                    case Directions.Left:
                        map[current.Block.Position.Y, current.Block.Position.X] = '<';
                        break;
                    case Directions.Down:
                        map[current.Block.Position.Y, current.Block.Position.X] = 'V';
                        break;
                    case Directions.Right:
                        map[current.Block.Position.Y, current.Block.Position.X] = '>';
                        break;
                }
                current = pathGraph[current.PreviousBlockId];
            }

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (!char.IsDigit(map[y, x]))
                        Console.ForegroundColor = ConsoleColor.Green;

                    Console.Write(map[y, x]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }

        static void PrintPath()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Path:");
            var current = pathGraph[destinationId];
            do
            {
                Console.WriteLine($"( X {current.Block.Position.X} | Y {current.Block.Position.Y} ) - {current.Block.HeatLoss}");
                current = pathGraph[current.PreviousBlockId];
            } while (!current.Block.Equals(start));
        }

        static int GetOverallHeatLoss()
        {
            var current = pathGraph[destinationId];
            int heatLoss = 0;
            while (!current.Block.Equals(start))
            {
                heatLoss += current.Block.HeatLoss;
                current = pathGraph[current.PreviousBlockId];
            }
            return heatLoss;
        }



        static void AStar()
        {
            BlockEntry? currentBlockEntry = null;
            CityBlock edgeTarget = null;
            int newDirectionCount;
            int costToReachEdgeTarget;
            string edgeTargetId;

            do
            {
                currentBlockEntry = GetNextInQ();

                PrintWithIndent($"Processing ( X {currentBlockEntry.Block.Position.X} | Y {currentBlockEntry.Block.Position.Y} ) {currentBlockEntry.Id}:", 0);

                for (Directions d = Directions.Up; d <= Directions.Right; d++)
                {
                    // skip if going backwards
                    if (d == OppositeDirectionOf(currentBlockEntry.DirectionToReach))
                        continue;

                    // skip if too many moves in same direction
                    if (d == currentBlockEntry.DirectionToReach &&
                        currentBlockEntry.DirectionCount == directionalLimit)
                        continue;

                    // skip if no edge in this direction
                    if (!currentBlockEntry.Block.Edges.ContainsKey(d))
                        continue;

                    edgeTarget = currentBlockEntry.Block.Edges[d].Target;
                    newDirectionCount =
                        d == currentBlockEntry.DirectionToReach ?
                        currentBlockEntry.DirectionCount + 1
                        : 1;
                    edgeTargetId = CreateBlockEntryId(edgeTarget.Position, d, newDirectionCount);


                    // skip if Block has already been processed
                    if (pathGraph.ContainsKey(edgeTargetId))
                        continue;

                    PrintWithIndent($"Processing Edge {d}", 1);

                    costToReachEdgeTarget = currentBlockEntry.CostToReach + edgeTarget.HeatLoss;

                    // neighboring node already in q -> try update
                    if (QContains(edgeTargetId))
                    {
                        if (costToReachEdgeTarget < GetQEntry(edgeTargetId).CostToReach)
                        {
                            PrintWithIndent($"Node already in q -> update", 2);
                            UpdateQEntry(edgeTargetId, costToReachEdgeTarget);
                        }
                        else
                            PrintWithIndent($"Node already in q -> no update", 2);
                    }
                    // neighboring node not in q -> add
                    else
                    {
                        PrintWithIndent($"Node not in q -> add", 2);
                        AddToQ(new(
                            edgeTargetId,
                            edgeTarget,
                            currentBlockEntry.Id,
                            costToReachEdgeTarget,
                            d,
                            newDirectionCount));
                    }
                }
                pathGraph.Add(currentBlockEntry.Id, currentBlockEntry);
            }
            while (!currentBlockEntry.Block.Equals(destination));
            destinationId = currentBlockEntry.Id;
        }

        static Directions OppositeDirectionOf(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up: return Directions.Down;
                case Directions.Left: return Directions.Right;
                case Directions.Down: return Directions.Up;
                case Directions.Right: return Directions.Left;
                default: return Directions.Nowhere;
            }
        }

        private static void UpdateQEntry(
            string id,
            int newCostToReach)
        {
            var entryToUpdate = Q.Where((e) => e.Id == id).First();
            entryToUpdate.CostToReach = newCostToReach;
        }

        private static BlockEntry GetNextInQ()
        {
            var next = Q.OrderBy((e) => e.CombinedCost()).ToList().First();
            Q.Remove(next);
            return next;
        }

        private static bool QContains(string id)
        {
            return Q.Any(e => e.Id == id);
        }

        private static string CreateBlockEntryId((int x, int y) pos, Directions d, int directionCount)
        {
            return $"{pos.x};{pos.y};{d};{directionCount}";
        }

        private static BlockEntry GetQEntry(string id)
        {
            return Q.Where(e => e.Id == id).First();
        }

        private static void AddToQ(BlockEntry e)
        {
            Q.Add(e);
        }

        static void Init()
        {
            var input = File.ReadAllLines("Common/input17.txt");
            city = new CityBlock[input.Length, input[0].Length];
            //initialize nodes without edges
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    city[y, x] = new CityBlock(Convert.ToInt32(input[y][x]) - '0', HeuristicValue(x, y), x, y);
                }
            }

            start = city[0, 0];
            destination = city[city.GetLength(0) - 1, city.GetLength(1) - 1];

            //fill in edges
            for (int y = 0; y < city.GetLength(0); y++)
            {
                for (int x = 0; x < city.GetLength(1); x++)
                {
                    // connect top
                    if (y > 0)
                        city[y, x].Edges[Directions.Up] = new Edge(city[y - 1, x]);
                    // connect left
                    if (x > 0)
                        city[y, x].Edges[Directions.Left] = new Edge(city[y, x - 1]);
                    // connect bottom
                    if (y < city.GetLength(0) - 1)
                        city[y, x].Edges[Directions.Down] = new Edge(city[y + 1, x]);
                    // connect right
                    if (x < city.GetLength(1) - 1)
                        city[y, x].Edges[Directions.Right] = new Edge(city[y, x + 1]);
                }
            }

            //direction to reach does not matter at start
            AddToQ(
                new(
                id: CreateBlockEntryId(start.Position, Directions.Nowhere, 0),
                block: start,
                previousBlockId: "0",
                costToReach: 0,
                directionToReach: Directions.Nowhere,
                directionCount: 0)
                );
        }

        private static double HeuristicValue(int x, int y)
        {
            //return 0;
            int xDistance = city.GetLength(1) - x - 1;
            int yDistance = city.GetLength(0) - y - 1;
            return Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
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
