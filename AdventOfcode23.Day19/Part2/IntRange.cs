namespace AdventOfCode23.Day19;

internal class IntRange(int lowerBound, int upperBound)
{
    public IntRange():this(1, 4000) {}
    public int LowerBound { get; set; } = lowerBound;
    public int UpperBound { get; set; } = upperBound;
}
