namespace AdventOfCode23.Day20.Common;

internal class Broadcaster(string id, Queue<Pulse> pulseQ) : IModule
{
    public string Id { get; init; } = id;
    private readonly Queue<Pulse> Q = pulseQ;
    private List<string> ConnectedReceivers = new();

    public void ConnectReceiver(string id)
    {
        ConnectedReceivers.Add(id);
    }

    public void Process(Pulse p)
    {
        if (!p.Signal)
        {
            foreach (string receiverId in ConnectedReceivers)
            {
                Q.Enqueue(new Pulse(Id, false, receiverId));
            }
        }
    }
}
