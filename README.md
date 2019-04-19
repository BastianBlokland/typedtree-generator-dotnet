# TypedTree-Generator-Dotnet

[![Build](https://img.shields.io/azure-devops/build/bastian-blokland/TypedTree/4/master.svg)](https://dev.azure.com/bastian-blokland/TypedTree/_build/latest?definitionId=4&branchName=master)
[![Tests](https://img.shields.io/azure-devops/tests/bastian-blokland/TypedTree/4/master.svg)](https://dev.azure.com/bastian-blokland/TypedTree/_build/latest?definitionId=4&branchName=master)
[![codecov](https://codecov.io/gh/BastianBlokland/typedtree-generator-dotnet/branch/master/graph/badge.svg)](https://codecov.io/gh/BastianBlokland/typedtree-generator-dotnet)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

| Cli tool | Global tool | Core library |
|----------|-------------|--------------|
| [![NuGet](https://img.shields.io/nuget/v/TypedTree.Generator.Cli.svg)](https://www.nuget.org/packages/TypedTree.Generator.Cli/) | [![NuGet](https://img.shields.io/nuget/v/TypedTree.Generator.GlobalTool.svg)](https://www.nuget.org/packages/TypedTree.Generator.GlobalTool/) | [![NuGet](https://img.shields.io/nuget/v/TypedTree.Generator.Core.svg)](https://www.nuget.org/packages/TypedTree.Generator.Core/) |

Dotnet cli tool for generating treescheme files for use in the  [**TypedTree-editor**](https://github.com/bastianblokland/typedtree-editor)

## Description
To avoid having to handwrite treescheme files you can generated them based on the dotnet class
structure of your tree (for example a behaviour tree structure).

## Usage
There are 3 different ways to use the generator:
| Usecase | Project | Documentation |
|---------|---------|---------------|
| Build integration | [**Cli**](https://www.nuget.org/packages/TypedTree.Generator.Cli/) | [Cli Readme](https://github.com/BastianBlokland/typedtree-generator-dotnet/tree/master/src/TypedTree.Generator.Cli/readme.md) |
| Command line | [**GlobalTool**](https://www.nuget.org/packages/TypedTree.Generator.GlobalTool/) | [GlobalTool Readme](https://github.com/BastianBlokland/typedtree-generator-dotnet/tree/master/src/TypedTree.Generator.GlobalTool/readme.md) |
| Manual library integration | [**Core**](https://www.nuget.org/packages/TypedTree.Generator.Core/) | [Core Readme](https://github.com/BastianBlokland/typedtree-generator-dotnet/tree/master/src/TypedTree.Generator.Core/readme.md) |
