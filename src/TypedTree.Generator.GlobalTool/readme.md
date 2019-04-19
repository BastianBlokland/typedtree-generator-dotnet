# **TypedTree-Generator** Global tool.

Can be used to generated scheme files for the [**TypedTree-Editor**](https://bastian.tech/tree/)

## Installation

Install as a global tool.
```bash
dotnet tool install --global TypedTree.Generator.GlobalTool
```

## Usage
Invoke the tool using the `typedtree-generator` alias
```bash
typedtree-generator --assembly Path/To/Example.dll --output
Path/To/ai.treescheme.json --root-type Game.AI.IBehaviourNode
```

## Help
For additional info on the arguments run `typedtree-generator --help`

For more general documentation and examples visit the github project [**typedtree-generator-dotnet**](https://github.com/BastianBlokland/typedtree-generator-dotnet).
