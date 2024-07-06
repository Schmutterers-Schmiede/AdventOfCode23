using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode23.Day20.Common;

namespace AdventOfCode23.Day20;

public class Day20_Part1
{    
    static Dictionary<string, IModule> modules = new();
    static Queue<Pulse> PulseQ = new();
    static ButtonModule buttonModule;
    static int highCount = 0;
    static int lowCount = 0;

    public static void Run()
    {
        Init();
        for (int i = 0; i < 1000; i++) 
        {
            Console.WriteLine($"=============  PUSH {i + 1}  =============");
            PushButton();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"High count: {highCount}");
            Console.WriteLine($"Low count: {lowCount}");
            Console.WriteLine($"Result: {CalculateResult()}");
        }
    }

    static long CalculateResult()
    {
        return (long)highCount * (long)lowCount;
    }

    static void PushButton()
    {        
        buttonModule.Push();
        Simulate();

    }

    static void Simulate()
    {
        while (PulseQ.Count != 0)
        {
            Pulse p = PulseQ.Dequeue();
            string signal = p.Signal ? "high" : "low";
            Console.WriteLine($"{p.SourceId} -{signal}-> {p.DestinationId}");
            if (p.Signal) highCount++;
            else lowCount++;

            modules[p.DestinationId].Process(p);
        }
    }

    static void Init()
    {                
        var input = File.ReadAllLines("Common/input20.txt");

        // extract list of module IDs
        var moduleIds = new List<string>();
        foreach(var line in input)
        {
            moduleIds.Add(Regex.Match(line, "([%|&][a-z]*)|broadcaster").Value);
        }

        // extract list of connections
        List<string[]> connections = new();
        foreach (var line in input)
        {
            connections.Add(Regex.Match(line, "(?<=> ).*").Value.Split(", "));
        }

        // create modules        
        modules.Add("broadcaster", new Broadcaster("broadcaster", PulseQ));
        for (int i = 0; i < moduleIds.Count; i++)
        {            
            if (moduleIds[i].StartsWith("%"))
                modules.Add(moduleIds[i].Substring(1), new FlipFlop(moduleIds[i].Substring(1), PulseQ));                
            
            else if (moduleIds[i].StartsWith("&"))
                modules.Add(moduleIds[i].Substring(1), new Conjunction(moduleIds[i].Substring(1), PulseQ));

            if (moduleIds[i] != "broadcaster")
                moduleIds[i] = moduleIds[i].Substring(1);
        }

        // connect modules        
        for (int i = 0; i < moduleIds.Count; i++)
        {
            foreach (var connectionId in connections[i])
            {
                if (!modules.ContainsKey(connectionId))
                    modules.Add(connectionId, new Output(connectionId));

                IModule m = modules[connectionId];
                modules[moduleIds[i]].ConnectReceiver(m.Id);
                if (m is Conjunction)
                    ((Conjunction)m).RegisterSource(moduleIds[i]);                
            }
        }

        // create button module
        buttonModule = new ButtonModule(PulseQ);
    }    
}
