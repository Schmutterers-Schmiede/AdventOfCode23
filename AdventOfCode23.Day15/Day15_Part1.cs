namespace AdventOfCode23.Day15
{
    public class Day15_Part1
    {
        static List<string> commands;
        public static void Run()
        {
            Init();
            int verificationNumber = 0;
            int hash;
            foreach (var command in commands)
            {
                hash = CalculateCommandHash(command);
                verificationNumber += hash;
                Console.WriteLine($"{command} - {verificationNumber}");
            }
            Console.WriteLine($"verification number: {verificationNumber}");
        }

        static int CalculateCommandHash(string command)
        {
            int hash = 0;

            for(int i = 0; i < command.Length; i++)
            {
                hash += command[i];
                hash *= 17;
                hash %= 256;
            }
            return hash;
        }

        static void Init()
        {
            commands = File.ReadAllText("input1.txt").Split(',').ToList();
        }
    }
}
