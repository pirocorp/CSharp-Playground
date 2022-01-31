# C# Source Generators

A Source Generator is a new kind of component that C# developers can write that lets you do two major things:

1. Retrieve a Compilation object that represents all user code that is being compiled. This object can be inspected and you can write code that works with the syntax and semantic models for the code being compiled, just like with analyzers today.

2. Generate C# source files that can be added to a Compilation object during the course of compilation. In other words, you can provide additional source code as input to a compilation while the code is being compiled.

When combined, these two things are what make Source Generators so useful. You can inspect user code with all of the rich metadata that the compiler builds up during compilation, then emit C# code back into the same compilation that is based on the data you’ve analyzed!

Source generators run as a phase of compilation visualized below:

[!Source generator run](./Picture1.png)

## Source Generators and Ahead of Time (AOT) Compilation

Another characteristic of Source Generators is that they can help remove major barriers to linker-based and AOT (ahead-of-time) compilation optimizations. Many frameworks and libraries make heavy use of reflection or reflection-emit, such as System.Text.Json, System.Text.RegularExpressions, and frameworks like ASP.NET Core that discover and/or emit types from user code at runtime.

## Hello World, Source Generator edition

The goal is to let users who have installed this Source Generator always have access to a friendly “Hello World” message and all syntax trees available during compilation. They could invoke it like this:

```csharp
public class SomeClassInMyCode
{
    public void SomeMethodIHave()
    {
        HelloWorldGenerated.HelloWorld.SayHello(); // calls Console.WriteLine("Hello World!") and then prints out syntax trees
    }
}
```


## Create Source Generator

1. Create a .NET Standard library project and add ```Microsoft.CodeAnalysis.CSharp``` and ```Microsoft.CodeAnalysis.Analyzers``` NuGet packages. The key pieces of this is that the project can generate a NuGet package and it depends on the bits that enable Source Generators.

2. Modify or create a C# file that specifies your own Source Generator. You’ll need to apply the ```Microsoft.CodeAnalysis.Generator``` attribute and implement the ```Microsoft.CodeAnalysis.ISourceGenerator``` interface.

```csharp
namespace MyGenerator
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Xml;
	
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.Text;

    [Generator]
    public class MySourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // TODO - actual source generator goes here!
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}
```

3. Add generated source code to the compilation!

4. Add the source generator from a project as an analyzer. When you write your code in Visual Studio, you’ll see that the Source Generator runs and the generated code is available to your project. You can now access it as if you had created it yourself:


**Note: you will currently need to restart Visual Studio to see IntelliSense and get rid of errors with the early tooling experience**

* [Source Generators design document](https://github.com/dotnet/roslyn/blob/master/docs/features/source-generators.md), which explains the Source Generator API and current capabilities

* [Source Generators Cookbook](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md)


