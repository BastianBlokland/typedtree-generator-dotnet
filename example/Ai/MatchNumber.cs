using System;
using System.Collections.Generic;

namespace Example.Ai
{
    public class MatchNumber : INode
    {
        private readonly int number;

        public MatchNumber(int number) =>
            this.number = number;

        public NodeResult Evaluate(string input)
        {
            if (!int.TryParse(input, out var inputNumber))
                return NodeResult.Failure;

            return inputNumber == this.number ? NodeResult.Success : NodeResult.Failure;
        }

        public void Reset()
        {

        }
    }
}
