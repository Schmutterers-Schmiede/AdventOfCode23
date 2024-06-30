namespace AdventOfCode23.Day20;

internal abstract class Module(string id)
{
    public string Id { get; init; } = id;
    public bool ReadyToSend { get; protected set; }    
    public List<Module> ConnectedReceivers { get; }

    public abstract void Receive(ReceiverParams p);
    public abstract void Send();    
    public abstract void ConnectReceiver(Module m);    
}
