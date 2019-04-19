using System;
using System.Collections.Generic;
using System.Linq;

namespace TypedTree.Generator.Core.Utilities
{
    /// <summary>
    /// Utility extensions for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Remove duplicates (or empty) lines from a string.
        /// </summary>
        /// <param name="value">String to distinct the lines for</param>
        /// <param name="linePrefix">Prefix to add in-front of lines</param>
        /// <returns>New string with unique lines</returns>
        public static string ToDistinctLines(this string value, string linePrefix = "")
        {
            var lines = value.Split(Environment.NewLine.ToCharArray());
            var nonEmptyLines = lines.Where(l => !string.IsNullOrEmpty(l));
            var distinctLines = nonEmptyLines.Distinct();
            return string.Join($"{Environment.NewLine}{linePrefix}", distinctLines);
        }
    }
}
