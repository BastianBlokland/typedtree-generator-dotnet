namespace Example.Ai
{
    public class Inverter : INode
    {
        private readonly INode child;

        public Inverter(INode child) =>
            this.child = child;

        public NodeResult Evaluate(string input)
        {
            var childResult = this.child.Evaluate(input);
            switch (childResult)
            {
                case NodeResult.Success:
                    return NodeResult.Failure;
                case NodeResult.Failure:
                    return NodeResult.Success;
            }

            return NodeResult.Running;
        }

        public void Reset() => this.child.Reset();
    }
}
