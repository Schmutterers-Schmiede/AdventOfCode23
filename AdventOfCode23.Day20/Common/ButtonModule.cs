namespace AdventOfCode23.Day20.Common;

internal class ButtonModule(Queue<Pulse> pulseQ)
{
    private readonly Queue<Pulse> Q = pulseQ;

    public void Push()
    {
        Q.Enqueue(new Pulse("Button", false, "broadcaster"));
    }
}
