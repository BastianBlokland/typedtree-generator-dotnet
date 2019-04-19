# **TypedTree-Generator** Example.

Very simple example showing a command-line bot using a form of a [**Behaviour tree**](https://en.wikipedia.org/wiki/Behavior_tree))

*NOTE: The behaviour-tree classes in this example are purely as an example, the library doesn't place any restrictions on how you structure your tree data.*

The types in the `Examples.Ai` namespace are forming the tree with `Example.Ai.INode` being the root interface.

In the csproj there is a post-build command that generates the `brain.treescheme.json`
```xml
<Target Name="GenerateBrainScheme" AfterTargets="Build">
<Exec Command="dotnet typedtree-generator \
-a $(OutputPath)Example.dll \
-r Example.Ai.INode \
-s PublicConstructorParameters \
-o brain.treescheme.json"/>
</Target>
```

Then the `brain.treescheme.json` can be loaded in the [**TypedTree Editor**](https://bastian.tech/tree)

![Example image](https://bastian.tech/media/typedtree-generator-dotnet.example.png)

After loading the scheme you can create a new tree in the editor or load the `brain.tree.json` for editing.

Then at runtime the tree structure can be loaded with a json orm (in this case [**Newtonsoft.Json**](https://www.newtonsoft.com/json))

Loading the tree using `Newtonsoft.Json`:
```c#
var brain = JsonConvert.DeserializeObject<Ai.Sequence>(brainJson, new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.Auto,
    SerializationBinder = new SerializationBinder()
});

class SerializationBinder : ISerializationBinder
{
    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        assemblyName = null;
        typeName = serializedType.FullName;
    }

    public Type BindToType(string assemblyName, string typeName) => Type.GetType(typeName);
}
```
Reason why we need the custom `ISerializationBinder` is that by default `Newtonsoft.Json` wants assembly data along with the types.
