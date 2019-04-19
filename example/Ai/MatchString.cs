using System;
using System.Collections.Generic;

namespace Example.Ai
{
    public class MatchString : INode
    {
        public enum Mode
        {
            MatchCase,
            IgnoreCase
        }

        private readonly string text;
        private readonly Mode mode;

        public MatchString(string text, Mode mode)
        {
            this.text = text;
            this.mode = mode;
        }

        public NodeResult Evaluate(string input)
        {
            var match = false;
            switch (this.mode)
            {
                case Mode.MatchCase:
                    match = input.Equals(this.text, StringComparison.InvariantCulture);
                    break;
                case Mode.IgnoreCase:
                    match = input.Equals(this.text, StringComparison.InvariantCultureIgnoreCase);
                    break;
            }

            return match ? NodeResult.Success : NodeResult.Failure;
        }

        public void Reset()
        {

        }
    }
}
