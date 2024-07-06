using System.Data;

namespace AdventOfCode23.Day20.Common;

internal class Conjunction(string id, Queue<Pulse> pulseQ) : IModule
{
    public string Id { get; init; } = id;
    private readonly Queue<Pulse> Q = pulseQ;

    private Dictionary<string, bool> Memory = new();
    List<string> ConnectedReceivers = new();


    public void Process(Pulse p)
    {
        Memory[p.SourceId] = p.Signal;
        bool signal = !Memory.All(x => x.Value);
        foreach (var receiverId in ConnectedReceivers)
        {
            Q.Enqueue(new Pulse(Id, signal, receiverId));
        }
    }

    public void ConnectReceiver(string id)
    {
        ConnectedReceivers.Add(id);
    }

    public void RegisterSource(string id)
    {
        Memory.Add(id, false);
    }
}
