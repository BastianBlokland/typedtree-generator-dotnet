using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load brain.
            var brainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "brain.tree.json");
            var brainJson = File.ReadAllText(brainPath);
            var brain = JsonConvert.DeserializeObject<Ai.Sequence>(brainJson, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new SerializationBinder()
            });

            var input = "";
            do
            {
                if (!string.IsNullOrEmpty(input))
                {
                    // Run brain.
                    brain.Reset();
                    while (brain.Evaluate(input) == Ai.NodeResult.Running);
                }

                // Get input.
                Console.WriteLine("-- Say something:");
                input = Console.ReadLine();
            } while (!string.IsNullOrEmpty(input) && input != "exit");
        }

        private class SerializationBinder : ISerializationBinder
        {
            public void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.FullName;
            }

            public Type BindToType(string assemblyName, string typeName) => Type.GetType(typeName);
        }
    }
}
