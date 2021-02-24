# Light.EmbeddedResources

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](https://github.com/feO2x/Light.EmbeddedResources/blob/master/LICENSE)
[![NuGet](https://img.shields.io/badge/NuGet-1.1.0-blue.svg?style=for-the-badge)](https://www.nuget.org/packages/Light.EmbeddedResources/)

### Get hold of embedded streams the easy way...

Are you one of the C# devs that doesn't know by heart how to get hold of an embedded resource? Light.EmbeddedResources makes it easy for you via extension methods:

```csharp
// Either get it as a stream
using var stream = this.GetEmbeddedStream("EmailTemplate.html");

// ...or as a string
string value = this.GetEmbeddedResource("EmailTemplate.html");
```

If you work in a static method, you can use types:

```csharp
using var stream = typeof(MyCurrentType).GetEmbeddedStream("EmailTemplate.html");
string value = typeof(MyCurrentType).GetEmbeddedResource("EmailTemplate.html");
```

Just make sure that the instance / the type you call `GetEmbeddedStream` or `GetEmbeddedResource` on is in the same assembly and namespace as the resource you want to retrieve.

### ...or copy them to files

You need to prepare a context, e.g. when you dynamically create PDF files out of prepared templates and the user data? You can easily copy your embedded resources to the file system:

```csharp
this.CopyEmbeddedStreamToFile("EmailTemplate.html", Path.Combine(targetDirectory, "template.html"));
```

Of course, there is also an overload that works with types.

### How to structure your code

I tend to have my embedded resource files directly next to the types that use them, so that I can simply call the extension methods on the target instances. My folder structure could e.g. look like this:

```
- Contracts
  |- ContractEmailSender.cs // a class that uses embedded resources
  |- Contract.html // embedded email template that will be converted to a PDF file
  |- company-logo.png // logo that will be embedded in the html file via the HTML-to-PDF engine
```

If you prefer to have all your embedded resources in one folder or in different assemblies, then create a type next to them to allow easy access:

```
- EmbeddedResources
  |- Resource1.json
  |- Resource2.xml
  |- Resources.cs
- OtherNamespace
  |- SomeService.cs // This service needs to access EmbeddedResources in another namespace / assembly
```

In this case, Resources and the client call site could be implemented like this:
```csharp
public readonly struct Resources
{
    public static readonly Instance = new Resources();
}

public class SomeService
{
    public void SomeFunction()
    {
        using var resource1Stream = Resources.Instance.GetEmbeddedStream("Resource1.json");
    }
}
```

## Supported Frameworks

Light.EmbeddedResources is compiled for

- .NET Standard 2.0
- .NET 5.0

and runs on all platforms that support these (like full .NET Framework, Mono, Xamarin, Unity, UWP, etc.).

## How to install

Light.EmbeddedResources is available as a [NuGet package](https://www.nuget.org/packages/Light.EmbeddedResources/):

- via csproj: `<PackageReference Include="Light.EmbeddedResources" Version="1.1.0" />`
- via .NET CLI: `dotnet add package Light.EmbeddedResources --version 1.1.0`
- via VS Package Manager: `Install-Package Light.EmbeddedResources -Version 1.1.0`
