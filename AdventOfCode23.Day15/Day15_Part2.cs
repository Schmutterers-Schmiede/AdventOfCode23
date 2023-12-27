using System.Text.RegularExpressions;

namespace AdventOfCode23.Day15
{
    public class Day15_Part2
    {
        static List<string> commands;
        static List<Box> boxes;
        public static void Run()
        {
            Init();
            int verificationNumber = 0;
            int hash;
            foreach (var command in commands)
            {
                Execute(command);
            }
            int focusingPower = CalculateFocusingPower();
            Console.WriteLine($"focusing power: {focusingPower}");
        }

        static int CalculateFocusingPower()
        {            
            int focusingPower = 0;
            int singleLensFocusingPower;
            for(int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].Lenses.Count > 0)
                {
                    Console.WriteLine($"===== Box {i,3} =====");
                    for (int j = 0; j < boxes[i].Lenses.Count; j++)
                    {
                        singleLensFocusingPower = (1 + i) * (j + 1) * boxes[i].Lenses[j].FocalLength;
                        focusingPower += singleLensFocusingPower;
                        Console.WriteLine($"{boxes[i].Lenses[j].Label, 4}: {1 + i, 3} {$"(box {i})"} * {j + 1} {$"(slot {j + 1})"} * {boxes[i].Lenses[j].FocalLength} (focal lenght) = {singleLensFocusingPower}");
                    }
                    Console.WriteLine();
                }
            }
            return focusingPower;
        }

        static void Execute(string command)
        {
            string labelRegexPattern = "[a-z]*";
            Regex labelRegex = new Regex(labelRegexPattern);

            Match labelMatch = labelRegex.Match(command);
            if(labelMatch.Success )
            {
                string lensLabel = labelMatch.Value;
                int boxIndex = CalculateLabelHash(lensLabel);

                if (command.Contains('-'))
                {
                    boxes[boxIndex].Lenses.RemoveAll(l => l.Label == lensLabel);
                }
                if (command.Contains('='))
                {
                    Regex focalLengthRegex = new Regex("[0-9]+");
                    Match focalLengthMatch = focalLengthRegex.Match(command);
                    int focalLength = Convert.ToInt32(focalLengthMatch.Value);
                    int index = boxes[boxIndex].Lenses.FindIndex(l => l.Label == lensLabel);

                    if (index == -1)
                        boxes[boxIndex].Lenses.Add(new Lens(lensLabel, focalLength));
                    else
                        boxes[boxIndex].Lenses[index] = new Lens(lensLabel, focalLength);
                }
            }
            else
            {
                throw new ArgumentException($"invaild command '{command}'");
            }
        }

        static int CalculateLabelHash(string input)
        {
            int hash = 0;

            for(int i = 0; i < input.Length; i++)
            {
                hash += input[i];
                hash *= 17;
                hash %= 256;
            }
            return hash;
        }

        static void Init()
        {
            commands = File.ReadAllText("input1.txt").Split(',').ToList();
            boxes = new List<Box>();
            for(int i = 0; i < 256; i++)
            {
                boxes.Add(new Box());
            }
        }
    }
}
