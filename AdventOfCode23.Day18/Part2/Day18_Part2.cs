using AdventOfCode23.Day18.Common;
using System.Drawing;
using System.Text.RegularExpressions;


namespace AdventOfCode23.Day18
{
    // algorithm explanation: https://www.youtube.com/watch?v=0KjG8Pg6LGk&t=99s
    public class Day18_Part2
    {
        static List<int> edgeLengths = new();
        static List<Directions> directions = new();
        static List<LongPoint> corners = new();
        static LongPoint digger = new LongPoint(0, 0);

        public static void Run()
        {
            Init();
            long area = CalculateArea();            
            Console.WriteLine($"Area: {area}");
        }

        static long CalculateArea()
        {
            return CalculateInnerArea() + (CalculatePerimeterArea() / 2) + 1;
        }

        static long CalculatePerimeterArea()
        {
            int perimeter = 0;
            for (int i = 0; i < edgeLengths.Count; i++)
            {
                perimeter += edgeLengths[i];                                
            }
            Console.WriteLine($"Perimeter: {perimeter}");
            return perimeter;
        }

        private static long CalculateInnerArea()
        {
            //get point coordinates
            var start = new LongPoint(0,0);
            digger = start;

            corners.Add(start);
            for (int i = 0; i < edgeLengths.Count; i++) 
            {
                Move(directions[i], edgeLengths[i]);
                corners.Add(new LongPoint(digger.X, digger.Y));
            }            

            //calculate inner area with shoelace algorithm
            long innerArea = 0;            
            for (int i = 0; i < corners.Count - 1; i++)
            {
                Console.WriteLine($"Point a ({i}): ( X {corners[i].X} | Y {corners[i].Y}");
                Console.WriteLine($"Point b ({i + 1}): ( X {corners[i + 1].X} | Y {corners[i + 1].Y}");

                var axby = corners[i].X * corners[i + 1].Y;
                var aybx = corners[i].Y * corners[i + 1].X;

                Console.WriteLine($"a.x * b.y = {axby}");
                Console.WriteLine($"a.y * b.x = {aybx}");

                var crossProduct = axby - aybx;

                Console.WriteLine($"Cross product: {crossProduct}");

                Console.WriteLine();
                innerArea += crossProduct;
            }
            innerArea = Math.Abs(innerArea) / 2;
            Console.WriteLine($"Inner area = {innerArea}");
            return innerArea;
        }

        static void Move(Directions d, int distance)
        {            
            switch (d)
            {
                case Directions.Right:
                    digger = new (digger.X + distance, digger.Y);
                    break;
                case Directions.Down:
                    digger = new (digger.X, digger.Y - distance);
                    break;
                case Directions.Left:
                    digger = new (digger.X - distance, digger.Y);
                    break;
                case Directions.Up:
                    digger = new (digger.X, digger.Y + distance);
                    break;
            }
        }

        private static void Init()
        {
            var instructions = File.ReadAllLines("Common/input.txt");
            foreach (var line in instructions)
            {                
                edgeLengths.Add(
                    Convert.ToInt32(
                        Regex.Match(
                            line, "([0-9a-fA-F]{5})").Value, 16
                            ));
                var direction = (Directions)line[line.Length - 2] - '0';
                directions.Add( direction );
            }
        }
    }
}
