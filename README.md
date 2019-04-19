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

## Example
An example can be found in the [**Example**](https://github.com/BastianBlokland/typedtree-generator-dotnet/tree/master/example) directory.

## Supported types
### Primitives
| Dotnet type | Treescheme type | Comments |
|-------------|-----------------|----------|
| `string` | String | |
| `bool` | Boolean | |
| `byte` | Number | Scheme does **not** have any guards to keep value in bounds |
| `sbyte` | Number | Scheme does **not** have any guards to keep value in bounds |
| `short` | Number | Scheme does **not** have any guards to keep value in bounds |
| `ushort` | Number | Scheme does **not** have any guards to keep value in bounds |
| `int` | Number | Scheme does **not** have any guards to keep value in bounds |
| `uint` | Number | Scheme does **not** have any guards to keep value in bounds |
| `long` | Number | Scheme does **not** have any guards to keep value in bounds |
| `ulong` | Number | Scheme does **not** have any guards to keep value in bounds |
| `float` | Number | Scheme does **not** have any guards to keep value in bounds |
| `double` | Number | Scheme does **not** have any guards to keep value in bounds |

### Enums
| Dotnet type | Treescheme type | Comments |
|-------------|-----------------|----------|
| Custom `enum` | Enum | Value of enum has to be convertible to a int |

### Classes
| Dotnet type | Treescheme type | Comments |
|-------------|-----------------|----------|
| Custom `class` | Node | Fields are found based on the provided `FieldSource` |
| Custom `struct` | Node | Fields are found based on the provided `FieldSource` |

### References
When a field references a `class` / `struct` / `interface`

| Dotnet type | Treescheme type | Comments |
|-------------|-----------------|----------|
| `class` | Alias | |
| `struct` | Alias | |
| `interface` | Alias | |

### Collections
| Dotnet type | Treescheme type | Comments |
|-------------|-----------------|----------|
| `T[]` | Array | |
| `IReadOnlyList<T>` | Array | |
| `IReadOnlyCollection<T>` | Array | |
| `ICollection<T>` | Array | |
| `IList<T>` | Array | |
