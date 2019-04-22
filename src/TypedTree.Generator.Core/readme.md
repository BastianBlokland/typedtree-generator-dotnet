# **TypedTree-Generator** Core library.

Library that can be used to generate scheme files for the [**TypedTree-Editor**](https://bastian.tech/tree/)

Can be used for more complex integration into a build pipeline, for simple use-cases consider one of the pre-made tools:
* [**TypedTree.Generator.Cli**](https://www.nuget.org/packages/TypedTree.Generator.Cli/)
* [**TypedTree.Generator.GlobalTool**](https://www.nuget.org/packages/TypedTree.Generator.GlobalTool/)

## Installation

There are two ways to add the nuget package:
1. Run:
```bash
dotnet add package TypedTree.Generator.Core --version 1.1.*
```
2. Add the following to a `ItemGroup` section of your csproj:
```xml
<PackageReference Include="TypedTree.Generator.Core" Version="1.1.*" />
```

## Usage
Steps for generating a scheme:
1. Add usings:
```c#
using TypedTree.Generator.Core.Mapping;
using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Serialization;
using TypedTree.Generator.Core.Utilities;
```
2. Create a ITypeCollection:
```c#
// Get an Assembly (or multiple) that will be used to look for types.
var typeCollection = TypeCollection.Create(typeof(TypeCollectionTests).Assembly);
```
3. Create a mapping context:
```c#
// Create context object containing all the settings for the mapping.
var context = Context.Create(typeCollection, FieldSource.PublicConstructorParameters);
```
Note: To get more diagnostics you can also supply a `Microsoft.Extensions.Logging.ILogger` implementation here.
4. Map the scheme:
```c#
var treeDefinition = TreeMapper.MapTree(context, rootAliasType: typeof(Ai.INode));
```
5. Export the json:
```c#
var json = JsonSerializer.ToJson(tree, JsonSerializer.Mode.Pretty);
```

## Help
For more general documentation and examples visit the github project [**typedtree-generator-dotnet**](https://github.com/BastianBlokland/typedtree-generator-dotnet).
