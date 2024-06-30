using System.Text.RegularExpressions;
using AdventOfCode23.Day19.Common;

namespace AdventOfCode23.Day19;

public class Day19_Part1
{
    static List<Part> parts = new();
    static Dictionary<string, Workflow> workflows = new();
    static List<Part> acceptedParts = new();

    public static void Run()
    {
        Init();        
        ProcessParts();
        Console.WriteLine();
        Console.WriteLine($"Result: {GetResult()}");
    }

    static int GetResult()
    {
        int sum = 0;
        foreach (var part in acceptedParts)
        {
            sum += part.X + part.M + part.A + part.S; 
        }
        return sum;
    }

    static void ProcessParts()
    {
        foreach (Part part in parts)
        {
            ProcessPart(part);
        }
    }

    static void ProcessPart(Part part)
    {        
        var workflow = workflows["in"];
        bool accepted = false;
        bool rejected = false;
        PrintWithIndent($"Processing part: X {part.X} M {part.M} A {part.A} S {part.S}");
        while (!accepted && !rejected)
        {            
            bool pass = false;
            int i = 0;
            while (i < workflow.RuleProperties.Count)
            {
                pass = ApplyRule(
                    part,
                    workflow.RuleProperties[i],
                    workflow.RuleOperators[i],
                    workflow.RuleValues[i]
                );
                if (pass) break;
                i++;
            }
            string nextTransfer;

            if (i == workflow.RuleProperties.Count)
                nextTransfer = workflow.FinalTransfer;
            else
                nextTransfer = workflow.RuleTransfers[i];

            if (nextTransfer == "A")
            {
                acceptedParts.Add(part);
                PrintWithIndent("Accepted", 2);
                accepted = true;
                continue;
            }
            if (nextTransfer == "R")
            {
                rejected = true;
                PrintWithIndent("Rejected", 2);
                continue;
            }

            workflow = workflows[nextTransfer];
        }
    }

    static bool ApplyRule(Part part, char p, char o, int value)
    {
        int property;
        switch (p)
        {
            case 'x':
                property = part.X;
                break;
            case 'm':
                property = part.M;
                break;
            case 'a':
                property = part.A;
                break;
            case 's':
                property = part.S;
                break;
            default:
                throw new Exception("property name not found");
        }

        if (o == '>') return property > value;
        else return property < value;
    }

    static void Init()
    {
        string[] instructions = File.ReadAllLines("Common/input19.txt");
        int splitIndex = Array.FindIndex(instructions, String.IsNullOrEmpty);
        string[] workflowLines = instructions.Take(splitIndex).ToArray();
        string[] partLines = instructions.Skip(splitIndex + 1).ToArray();

        // init workflows
        string finalTransfer;
        foreach (var line in workflowLines)
        {
            var id = Regex.Match(line, "[a-z]{2,3}(?={)").Value;                

            var rules = Regex.Match(line, "(?<={).*(?=,[a-zA-Z]*})").Value.Split(',');
            List<char> ruleProperties = new();
            List<char> ruleOperators = new();
            List<int> ruleValues = new();
            List<string> ruleTransfers = new();
            foreach(var rule in rules)
            {
                ruleProperties.Add(Regex.Match(rule, "[a-z](?=<|>)").Value[0]);
                ruleOperators.Add(Regex.Match(rule, "<|>").Value[0]);                
                ruleValues.Add(Convert.ToInt32(Regex.Match(rule, "(?<=[<|>])[0-9]*").Value));
                ruleTransfers.Add(Regex.Match(rule, "(?<=:).*").Value);
            }
            finalTransfer = Regex.Match(line, "[a-zA-Z]*(?=})").Value;

            workflows.Add(id, new Workflow(
                id,
                ruleProperties,
                ruleOperators,
                ruleValues,
                ruleTransfers,
                finalTransfer
                ));
        }

        // init parts
        foreach (var line in partLines)
        {
            var x = Convert.ToInt32(Regex.Match(line, "(?<=x=)[0-9]*").Value);
            parts.Add(new(
                Convert.ToInt32(Regex.Match(line, "(?<=x=)[0-9]*").Value),
                Convert.ToInt32(Regex.Match(line, "(?<=m=)[0-9]*").Value),
                Convert.ToInt32(Regex.Match(line, "(?<=a=)[0-9]*").Value),
                Convert.ToInt32(Regex.Match(line, "(?<=s=)[0-9]*").Value)
                ));
        }
    }    
    
    static void PrintWithIndent(string message, int indentLevel = 0)
    {
        for (int i = 0; i < indentLevel; i++)
        {
            Console.Write("  ");
        }
        Console.WriteLine(message);
    }
}
