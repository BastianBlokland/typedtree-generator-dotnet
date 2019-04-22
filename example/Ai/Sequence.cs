using System.Collections.Generic;

namespace Example.Ai
{
    /// <summary>
    /// Executes child nodes, will stop at the first child that returns failure.
    /// </summary>
    internal class Sequence : INode
    {
        private readonly IReadOnlyList<INode> children;

        public Sequence(IReadOnlyList<INode> children) =>
            this.children = children;

        public NodeResult Evaluate(string input)
        {
            foreach (var child in this.children)
            {
                var childResult = child.Evaluate(input);
                switch (childResult)
                {
                    case NodeResult.Running:
                        return NodeResult.Running;
                    case NodeResult.Failure:
                        return NodeResult.Failure;
                }
            }

            return NodeResult.Success;
        }

        public void Reset()
        {
            foreach (var child in this.children)
                child.Reset();
        }
    }
}
