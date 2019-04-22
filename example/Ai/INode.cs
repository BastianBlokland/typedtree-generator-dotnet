namespace Example.Ai
{
    internal interface INode
    {
        NodeResult Evaluate(string input);

        void Reset();
    }
}
