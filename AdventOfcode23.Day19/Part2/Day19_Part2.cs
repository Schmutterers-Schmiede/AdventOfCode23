using AdventOfCode23.Day19.Common;
using AdventOfCode23.Day19.Part2;
using System.ComponentModel;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day19;

public class Day19_Part2
{
    static Dictionary<string, Workflow> workflows = new();
    static PartRange rootPartRange;
    static List<PartRange> acceptedParts = new();
    static BigInteger accepted;
    public static void Run()
    {
        Init();
        FindAcceptedParts(workflows["in"], rootPartRange);
        CalculateAcceptedParts();
        Console.WriteLine($"Total possible Combinations: {accepted}");
    }    

    static void FindAcceptedParts(Workflow workflow, PartRange partRange)
    {        
        IntRange passingRange;        
        for (int i = 0; i < workflow.RuleProperties.Count; i++)
        {                        
            switch (workflow.RuleProperties[i])
            {
                case 'x':
                    passingRange = PassingRange(
                        partRange.X, 
                        workflow.RuleOperators[i], 
                        workflow.RuleValues[i]);

                    partRange.X = FailingRange(partRange.X, passingRange);

                    if(!IntRangeIs0(passingRange))
                    {
                        var newPartRange = new PartRange(passingRange, partRange.M, partRange.A, partRange.S);
                        if (workflow.RuleTransfers[i] == "A")
                        {
                            acceptedParts.Add(newPartRange);
                            LogAcceptedPartRange(newPartRange);
                        }
                        else if (workflow.RuleTransfers[i] != "R")
                        {                            
                            FindAcceptedParts(
                                workflows[workflow.RuleTransfers[i]], 
                                new(passingRange, partRange.M, partRange.A, partRange.S));
                        }
                    }                    
                    break;

                case 'm':
                    passingRange = PassingRange(
                        partRange.M,
                        workflow.RuleOperators[i],
                        workflow.RuleValues[i]);

                    partRange.M = FailingRange(partRange.M, passingRange);

                    if (!IntRangeIs0(passingRange))
                    {
                        var newPartRange = new PartRange(partRange.X, passingRange, partRange.A, partRange.S);
                        if (workflow.RuleTransfers[i] == "A")
                        {
                            acceptedParts.Add(newPartRange);
                            LogAcceptedPartRange(newPartRange);
                        }
                        else if (workflow.RuleTransfers[i] != "R")
                        {
                            FindAcceptedParts(
                                workflows[workflow.RuleTransfers[i]],
                                new(partRange.X, passingRange, partRange.A, partRange.S));
                        }
                    }
                    break;

                case 'a':
                    passingRange = PassingRange(
                        partRange.A,
                        workflow.RuleOperators[i],
                        workflow.RuleValues[i]);

                    partRange.A = FailingRange(partRange.A, passingRange);

                    if (!IntRangeIs0(passingRange))
                    {
                        var newPartRange = new PartRange(partRange.X, partRange.M, passingRange, partRange.S);
                        if (workflow.RuleTransfers[i] == "A")
                        {
                            acceptedParts.Add(newPartRange);
                            LogAcceptedPartRange(newPartRange);
                        }
                        else if (workflow.RuleTransfers[i] != "R")
                        {                            
                            FindAcceptedParts(
                                workflows[workflow.RuleTransfers[i]],
                                new(partRange.X, partRange.M, passingRange, partRange.S));
                        }
                    }
                    break;

                case 's':
                    passingRange = PassingRange(
                        partRange.S,
                        workflow.RuleOperators[i],
                        workflow.RuleValues[i]);

                    partRange.S = FailingRange(partRange.S, passingRange);

                    if (!IntRangeIs0(passingRange))
                    {
                        var newPartRange = new PartRange(partRange.X, partRange.M, partRange.A, passingRange);
                        if (workflow.RuleTransfers[i] == "A")
                        {
                            acceptedParts.Add(newPartRange);
                            LogAcceptedPartRange(newPartRange);
                        }
                        else if (workflow.RuleTransfers[i] != "R")
                        {
                            FindAcceptedParts(
                             workflows[workflow.RuleTransfers[i]],
                             new(partRange.X, partRange.M, partRange.A, passingRange));
                        }
                    }
                    break;

                default:
                    throw new Exception("property name not found"); // will never be thrown
            }            
        }  
        if(!IntRangeIs0(partRange.X) && !IntRangeIs0(partRange.M) && !IntRangeIs0(partRange.A) && !IntRangeIs0(partRange.S))
        {
            var newPartRange = new PartRange(partRange.X, partRange.M, partRange.A, partRange.S);
            if (workflow.FinalTransfer == "A")
            {                
                acceptedParts.Add(newPartRange);
                LogAcceptedPartRange(newPartRange);
            }
            else if (workflow.FinalTransfer != "R")
                FindAcceptedParts(workflows[workflow.FinalTransfer], newPartRange);
        }
    }

    static void CalculateAcceptedParts()
    {        
        BigInteger x;
        foreach (var partRange in acceptedParts)
        {
            x = partRange.X.UpperBound - partRange.X.LowerBound + 1;
            x *= partRange.M.UpperBound - partRange.M.LowerBound + 1;
            x *= partRange.A.UpperBound - partRange.A.LowerBound + 1;
            x *= partRange.S.UpperBound - partRange.S.LowerBound + 1;
            accepted += x;
        }
    }

    static IntRange FailingRange(IntRange input, IntRange output) 
    {
        if (input.LowerBound == output.LowerBound && input.UpperBound == output.UpperBound)
            return new(0, 0);

        if(input.LowerBound == output.LowerBound) 
            return new(output.UpperBound + 1, input.UpperBound);

        if(input.UpperBound == output.UpperBound)
            return new(input.LowerBound, output.LowerBound - 1);

        return new(-1, -1); // impossible case
    }

    static IntRange PassingRange(IntRange prop, char op, int threshold)
    {        
        if(op == '>')
        {
            if(prop.LowerBound <= threshold)
            {
                if (prop.UpperBound <= threshold) 
                    return new(0, 0);
                if (prop.UpperBound > threshold) 
                    return new(threshold + 1, prop.UpperBound);
            }
            if (prop.LowerBound > threshold)
                return prop;            
        }
        else // <
        {
            if(prop.UpperBound >= threshold)
            {
                if (prop.LowerBound >= threshold)
                    return new(0, 0);
                if (prop.LowerBound < threshold)
                    return new(prop.LowerBound, threshold - 1);
            }
            if (prop.UpperBound < threshold)
                return prop;
        }
        return new(-1, -1); // impossible state
    }

    static void LogAcceptedPartRange(PartRange partRange)
    {
        Console.Write("Found accepted part range:    ");

        Console.Write(" X ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("{0,4} - {1,-6} ", partRange.X.LowerBound, partRange.X.UpperBound);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write(" M ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("{0,4} - {1,-6} ", partRange.M.LowerBound, partRange.M.UpperBound);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write(" A ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("{0,4} - {1,-6} ", partRange.A.LowerBound, partRange.A.UpperBound);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write(" S ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("{0,4} - {1,-6} ", partRange.S.LowerBound, partRange.S.UpperBound);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
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
            foreach (var rule in rules)
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
        var defaultIntRange = new IntRange();
        rootPartRange = new(defaultIntRange, defaultIntRange, defaultIntRange, defaultIntRange);
        accepted = 0;
    }

    static bool IntRangeIs0(IntRange range)
    {
        return range.UpperBound == 0 && range.LowerBound == 0;
    }
}
