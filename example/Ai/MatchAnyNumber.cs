using System;
using System.Collections.Generic;

namespace Example.Ai
{
    /// <summary>
    /// Returns successfull if the user typed any number.
    /// </summary>
    internal class MatchAnyNumber : INode
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
