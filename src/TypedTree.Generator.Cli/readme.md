# **TypedTree-Generator** Cli tool.

Can be used to generate scheme files for the [**TypedTree-Editor**](https://bastian.tech/tree/)

## Installation

Add a reference to the cli-tool to a `ItemGroup` section your of your csproj.
```xml
<DotNetCliToolReference Include="TypedTree.Generator.Cli" Version="2.0.*" />
```

## Usage
There two ways to invoke the tool:
1. Manually execute `dotnet typedtree-generator` from a command prompt.
2. Add a post-build step to your csproj file: (Outside of a `ItemGroup` or `PropertyGroup`)
```xml
<Target Name="GenerateTreeScheme" AfterTargets="Build">
    <Exec Command="dotnet typedtree-generator \
    -a $(OutputPath)NameOfAssembly.dll \
    -r NameOfRootType \
    -s PublicConstructorParameters \
    -o output.treescheme.json"/>
</Target>
```

## Help
For additional info on the arguments run `dotnet typedtree-generator --help`

For more general documentation and examples visit the github project [**typedtree-generator-dotnet**](https://github.com/BastianBlokland/typedtree-generator-dotnet).
