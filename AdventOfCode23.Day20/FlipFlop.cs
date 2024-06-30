
namespace AdventOfCode23.Day20;

internal class FlipFlop : Module
{
    private bool outputState;
    public FlipFlop(string id) : base(id) 
    {
        outputState = false;
    }
    public override void Receive(ReceiverParams p)
    {
        if (!p.Pulse)
        {
            outputState = !outputState;
            ReadyToSend = true;
        }
    }

    public override void Send()
    {
        foreach (var receiver in ConnectedReceivers)
        {
            receiver.Receive(new ReceiverParams(this, outputState));
        }
        ReadyToSend = false;
    }

    public override void ConnectReceiver(Module m)
    {
        ConnectedReceivers.Add(m);
    }
}
