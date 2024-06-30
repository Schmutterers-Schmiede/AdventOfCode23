namespace AdventOfCode23.Day19.Common;

internal class Workflow(
    string id,
    List<char> ruleProperties,
    List<char> ruleOperators,
    List<int> ruleValues,
    List<string> ruleTransfers,
    string finalTransfer)

{
    public string Id { get; init; } = id;
    public List<char> RuleProperties { get; init; } = ruleProperties;
    public List<char> RuleOperators { get; init; } = ruleOperators;
    public List<int> RuleValues { get; init; } = ruleValues;
    public List<string> RuleTransfers { get; init; } = ruleTransfers;
    public string FinalTransfer { get; init; } = finalTransfer;
}
