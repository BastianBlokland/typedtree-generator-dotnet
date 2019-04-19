using System;
using System.Collections.Generic;

namespace Example.Ai
{
    public class MatchAnyNumber : INode
    {
        public NodeResult Evaluate(string input)
        {
            return int.TryParse(input, out _) ? NodeResult.Success : NodeResult.Failure;
        }

        public void Reset()
        {

        }
    }
}
