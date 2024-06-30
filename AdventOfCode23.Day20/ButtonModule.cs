namespace AdventOfCode23.Day20;

internal class ButtonModule : Module
{
    public ButtonModule(string id) : base(id) {}
    public override void ConnectReceiver(Module m)
    {
        ConnectedReceivers.Add(m);
    }

    public override void Receive(ReceiverParams p)
    {
        throw new NotImplementedException();
    }

    public override void Send()
    {
        foreach (var receiver in ConnectedReceivers)
        {
            receiver.Receive(new ReceiverParams(this, false));
        }
    }
}
