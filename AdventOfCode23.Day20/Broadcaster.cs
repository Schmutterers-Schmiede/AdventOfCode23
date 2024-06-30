namespace AdventOfCode23.Day20;

internal class Broadcaster : Module
{
    public Broadcaster(string id) : base(id) { }    

    public override void ConnectReceiver(Module m)
    {
        ConnectedReceivers.Add(m);
    }

    public override void Receive(ReceiverParams p)
    {
        ReadyToSend = true;
    }

    public override void Send()
    {
        foreach (var receiver in ConnectedReceivers)
        {
            receiver.Receive(new ReceiverParams(this, false));
        }
        ReadyToSend = false;
    }
}
