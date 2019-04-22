using System.Collections.Generic;

namespace Example.Ai
{
    /// <summary>
    /// Executes child nodes, will stop at the first child that returns successful.
    /// </summary>
    internal class Selector : INode
    {
        private readonly IReadOnlyList<INode> children;

        public Selector(IReadOnlyList<INode> children) =>
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
                    case NodeResult.Success:
                        return NodeResult.Success;
                }
            }

            return NodeResult.Failure;
        }

        public void Reset()
        {
            foreach (var child in this.children)
                child.Reset();
        }
    }
}
