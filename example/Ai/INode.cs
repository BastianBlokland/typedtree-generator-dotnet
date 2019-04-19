namespace Example.Ai
{
    public interface INode
    {
        NodeResult Evaluate(string input);

        void Reset();
    }
}
