using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TypedTree.Generator.Core.Scheme;

namespace TypedTree.Generator.Core.Serialization
{
    /// <summary>
    /// Class containing utilities for serialzing scheme's to json.
    /// </summary>
    public static class JsonSerializer
    {
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        /// <summary>
        /// Mode to use when serialzing json.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Small non-formatted output.
            /// </summary>
            Normal = 0,

            /// <summary>
            /// Bigger formatted output.
            /// </summary>
            Pretty = 1
        }

        /// <summary>
        /// Create a json representation of a tree-scheme.
        /// </summary>
        /// <param name="tree">TreeScheme to create json for</param>
        /// <param name="mode">Mode to use when writing the json</param>
        /// <returns>Json representation of the tree</returns>
        public static string ToJson(this TreeDefinition tree, Mode mode = Mode.Pretty)
        {
            // Create a Json.Net 'JObject' representation of the tree.
            var jsonObject = CreateSchemeJsonObject(tree);

            // Create a json string.
            var formatting = GetFormattingSettings(mode);
            return jsonObject.ToString(formatting);
        }

        /// <summary>
        /// Create a json representation of a tree-scheme and write it as utf8 text to a stream.
        /// </summary>
        /// <param name="tree">TreeScheme to write json for</param>
        /// <param name="mode">Mode to use when writing the json</param>
        /// <param name="outputStream">Stream to write the json to</param>
        public static void WriteJson(this TreeDefinition tree, Mode mode, Stream outputStream) =>
            WriteJson(tree, mode, outputStream, Utf8NoBom, leaveOpen: false);

        /// <summary>
        /// Create a json representation of a tree-scheme and write it to a stream.
        /// </summary>
        /// <param name="tree">TreeScheme to write json for</param>
        /// <param name="mode">Mode to use when writing the json</param>
        /// <param name="outputStream">Stream to write the json to</param>
        /// <param name="encoding">Encoding to use when writing text</param>
        /// <param name="leaveOpen">Should the outputStream be left open or closed</param>
        public static void WriteJson(
            this TreeDefinition tree,
            Mode mode,
            Stream outputStream,
            Encoding encoding,
            bool leaveOpen)
        {
            // Create a Json.Net 'JObject' representation of the tree.
            var jsonObject = CreateSchemeJsonObject(tree);

            // Write json to the output stream.
            using (var writer = new StreamWriter(outputStream, encoding, bufferSize: 1024, leaveOpen))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                jsonWriter.Formatting = GetFormattingSettings(mode);

                var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
                serializer.Serialize(jsonWriter, jsonObject);
            }
        }

        private static JObject CreateSchemeJsonObject(TreeDefinition tree)
        {
            var obj = new JObject();
            obj["rootAlias"] = tree.RootAlias.Identifier;
            obj["aliases"] = new JArray(tree.Aliases.Select(CreateAliasJsonObject).ToArray());
            obj["enums"] = new JArray(tree.Enums.Select(CreateEnumJsonObject).ToArray());
            obj["nodes"] = new JArray(tree.Nodes.Select(CreateNodeJsonObject).ToArray());
            return obj;
        }

        private static JObject CreateAliasJsonObject(AliasDefinition alias)
        {
            var obj = new JObject();
            obj["identifier"] = alias.Identifier;
            obj["values"] = new JArray(alias.Values.ToArray());
            return obj;
        }

        private static JObject CreateEnumJsonObject(EnumDefinition @enum)
        {
            var obj = new JObject();
            obj["identifier"] = @enum.Identifier;
            obj["values"] = new JArray(@enum.Values.Select(CreateEntryJsonObject).ToArray());
            return obj;

            JObject CreateEntryJsonObject(EnumEntry entry)
            {
                var entryObj = new JObject();
                entryObj["value"] = entry.Value;
                entryObj["name"] = entry.Name;
                return entryObj;
            }
        }

        private static JObject CreateNodeJsonObject(NodeDefinition node)
        {
            var obj = new JObject();
            obj["nodeType"] = node.Type;
            obj["fields"] = new JArray(node.Fields.Select(CreateFieldJsonObject).ToArray());
            return obj;
        }

        private static JObject CreateFieldJsonObject(INodeField field)
        {
            var obj = new JObject();
            obj["name"] = field.Name;
            obj["valueType"] = GetValueTypeString();
            if (field.IsArray)
                obj["isArray"] = true;
            return obj;

            string GetValueTypeString()
            {
                switch (field)
                {
                    case StringNodeField nf: return "string";
                    case BooleanNodeField nf: return "boolean";
                    case NumberNodeField nf: return "number";
                    case AliasNodeField nf: return nf.Alias.Identifier;
                    case EnumNodeField nf: return nf.Enum.Identifier;
                }

                throw new InvalidOperationException($"Unknown field type: '{field.GetType()}'");
            }
        }

        private static Formatting GetFormattingSettings(Mode mode)
        {
            switch (mode)
            {
                case Mode.Pretty:
                    return Formatting.Indented;
                default:
                    return Formatting.None;
            }
        }
    }
}
