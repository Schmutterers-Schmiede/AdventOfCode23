namespace AdventOfCode23.Day20;

internal class Conjunction : Module
{
    public Conjunction(string id) : base(id)
    {
        Memory = new();
    }

    Dictionary<string, bool> Memory;

    public override void Receive(ReceiverParams p)
    {
        Memory[p.Sender.Id] = p.Pulse;
        ReadyToSend = true;
    }

    public override void Send()
    {        
        bool pulse = !AllInputsTrue();        
        foreach (var receiver in ConnectedReceivers)
        {
            receiver.Receive(new ReceiverParams(this, pulse));
        }
        ReadyToSend = false;
    }

    private bool AllInputsTrue()
    {
        return Memory.All((x) => x.Value);        
    }
    public override void ConnectReceiver(Module m)
    {
        ConnectedReceivers.Add(m);
        Memory.Add(m.Id, false);
    }
}
