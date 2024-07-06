using AdventOfCode23.Day20.Common;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day20;

public class Day20_Part2
{
    static Dictionary<string, IModule> modules = new();
    static Queue<Pulse> PulseQ = new();
    static ButtonModule buttonModule;

    static int pushCount = 0;
    static int hn = 0;
    static int vn = 0;
    static int kt = 0;
    static int ph = 0;
    static List<long> LCMInput = new();
    public static void Run()
    {
        Init();
        
        while (hn == 0 || vn == 0 || kt == 0 || ph == 0) 
        {
            pushCount++;
            Console.WriteLine($"=============  PUSH {pushCount}  =============");
            if (PushButton())
                break;
        }            
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        LCMInput.Add(hn);
        LCMInput.Add(vn);
        LCMInput.Add(kt);
        LCMInput.Add(ph);
        Console.WriteLine($"Result: {CalculateLCM()}");
    }

    static long CalculateLCM()
    {
        long lcm = LCMInput[0];

        for (int i = 1; i < LCMInput.Count; i++) 
        {
            lcm = LCM(lcm, LCMInput[i]);
        }

        return lcm;

        long LCM(long a, long b) 
        {
            return Math.Abs((a * b) / GCD(a, b));
        }

        long GCD(long a, long b)
        {
            while(b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }

    


    static bool PushButton()
    {        
        buttonModule.Push();

        if (Simulate()) 
            return true;

        return false;
    }

    static bool Simulate()
    {
        while (PulseQ.Count != 0)
        {
            Pulse p = PulseQ.Dequeue();
            string signal = p.Signal ? "high" : "low";

            if (p.Signal && p.SourceId == "hn" && hn == 0)            
                hn = pushCount;
            if (p.Signal && p.SourceId == "vn" && vn == 0)
                vn = pushCount;                
            if (p.Signal && p.SourceId == "kt" && kt == 0)
                kt = pushCount;                
            if (p.Signal && p.SourceId == "ph" && ph == 0)
                ph = pushCount;


            Console.WriteLine($"{p.SourceId} -{signal}-> {p.DestinationId}");                        

            modules[p.DestinationId].Process(p);
        }
        return false;
    }

    static void Init()
    {
        var input = File.ReadAllLines("Common/input20.txt");

        // extract list of module IDs
        var moduleIds = new List<string>();
        foreach (var line in input)
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
        for (int i = 0; i < moduleIds.Count; i++)
        {
            if (moduleIds[i].StartsWith("%"))
                modules.Add(moduleIds[i].Substring(1), new FlipFlop(moduleIds[i].Substring(1), PulseQ));

            else if (moduleIds[i].StartsWith("&"))
                modules.Add(moduleIds[i].Substring(1), new Conjunction(moduleIds[i].Substring(1), PulseQ));

            if (moduleIds[i] != "broadcaster")
                moduleIds[i] = moduleIds[i].Substring(1);

            if (moduleIds[i] == "broadcaster")
                modules.Add("broadcaster", new Broadcaster("broadcaster", PulseQ));
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
