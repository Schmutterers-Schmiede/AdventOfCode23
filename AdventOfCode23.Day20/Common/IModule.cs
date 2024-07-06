namespace AdventOfCode23.Day20.Common;

internal interface IModule
{
    string Id { get; init; }
    void Process(Pulse p);
    void ConnectReceiver(string id);
}
