using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Implementation of <see cref="ICommentProvider"/> that parses a doc-comment output xml file.
    /// </summary>
    public sealed class XmlCommentProvider : ICommentProvider
    {
        private readonly XmlDocument document;

        internal XmlCommentProvider(XmlDocument document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }

        /// <summary>
        /// Attempt to parse a <see cref="XmlCommentProvider"/> from given doc-comment stream.
        /// </summary>
        /// <param name="xmlStream">Stream containing doc-comment xml to parse</param>
        /// <param name="provider">Provider if parsed successfully, otherwise null</param>
        /// <returns>True if parsed successfully, otherwise false</returns>
        public static bool TryParse(Stream xmlStream, out XmlCommentProvider provider)
        {
            if (xmlStream == null)
                throw new ArgumentNullException(nameof(xmlStream));

            var doc = new XmlDocument();
            try
            {
                doc.Load(xmlStream);
                provider = new XmlCommentProvider(doc);
                return true;
            }
            catch (XmlException)
            {
                provider = default;
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse a <see cref="XmlCommentProvider"/> from given doc-comment xml string.
        /// </summary>
        /// <param name="xml">Xml containing doc-comment content to parse</param>
        /// <param name="provider">Provider if parsed successfully, otherwise null</param>
        /// <returns>True if parsed successfully, otherwise false</returns>
        public static bool TryParse(string xml, out XmlCommentProvider provider)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentException($"Invalid xml: '{xml}' provided", nameof(xml));

            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                provider = new XmlCommentProvider(doc);
                return true;
            }
            catch (XmlException)
            {
                provider = default;
                return false;
            }
        }

        /// <summary>
        /// Get a comment for given type.
        /// </summary>
        /// <param name="type">Type to get the comment for</param>
        /// <returns>Comment string if available, otherwise null</returns>
        public string GetComment(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Find the member element for given type using xpath.
            var typeDoc = this.document.SelectSingleNode(
                $"//member[starts-with(@name, 'T:{type.FullName}')]");
            if (typeDoc == null)
                return null;

            // Look for the summary element inside the member element.
            var summaryElement = typeDoc["summary"];
            if (summaryElement == null)
                return null;

            // Sanitize the output by stripping starting whitespace from all lines.
            return Regex.Replace(summaryElement.InnerText, @"(^\s+)", string.Empty, RegexOptions.Multiline);
        }
    }
}
