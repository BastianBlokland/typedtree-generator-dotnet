using System;
using System.Collections.Generic;

namespace Example.Ai
{
    /// <summary>
    /// Print given text to the commandline. Will wait for given time before continuing.
    /// </summary>
    internal class Say : INode
    {
        private readonly string text;
        private readonly float time;

        private DateTime? beginTime;

        public Say(string text, float time)
        {
            this.text = text;
            this.time = time;
        }

        public NodeResult Evaluate(string input)
        {
            if (this.beginTime == null)
            {
                Console.WriteLine(this.text);
                this.beginTime = DateTime.UtcNow;
            }

            var elapsed = DateTime.UtcNow - this.beginTime.Value;
            return elapsed > TimeSpan.FromSeconds(this.time) ? NodeResult.Success : NodeResult.Running;
        }

        public void Reset()
        {
            this.beginTime = null;
        }
    }
}
