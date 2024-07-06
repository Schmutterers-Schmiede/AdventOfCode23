
using System.Runtime.CompilerServices;

namespace AdventOfCode23.Day20.Common;

internal class FlipFlop(string id, Queue<Pulse> pulseQ) : IModule
{
    private readonly Queue<Pulse> Q = pulseQ;
    public string Id { get; init; } = id;
    private bool state = false;

    List<string> ConnectedReceivers = new();

    public void Process(Pulse p)
    {
        if (!p.Signal)
        {
            state = !state;
            foreach (var receiverId in ConnectedReceivers)
            {
                Q.Enqueue(new Pulse(Id, state, receiverId));
            }
        }
    }

    public void ConnectReceiver(string id)
    {
        ConnectedReceivers.Add(id);
    }
}
