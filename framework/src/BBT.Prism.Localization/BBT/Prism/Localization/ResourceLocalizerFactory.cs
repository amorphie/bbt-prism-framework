using System;
using System.Globalization;
using System.Reflection;
using BBT.Prism.Localization.Json;
using BBT.Prism.Localization.Resx;
using Microsoft.Extensions.Localization;

namespace BBT.Prism.Localization;

public class ResourceLocalizerFactory: IStringLocalizerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ResourceLocalizerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        var resourceType = GetResourceTypeFromConfiguration(resourceSource);

        switch (resourceType)
        {
            case "json":
                var jsonBaseName = $"{resourceSource.Namespace}.Resources.lang.{CultureInfo.CurrentUICulture.Name}.json";
                return new JsonStringLocalizer(jsonBaseName, resourceSource.Assembly);
            case "resx":
                var resxBaseName = $"{resourceSource.Namespace}.Resources.Resource";
                return new ResxStringLocalizer(resxBaseName, resourceSource.Assembly);
            default:
                throw new NotSupportedException($"Resource type '{resourceType}' is not supported.");
        }
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        var assembly = Assembly.Load(new AssemblyName(location));
        var resourceType = GetResourceTypeFromConfiguration(assembly.GetType(baseName)!);

        switch (resourceType)
        {
            case "json":
                return new JsonStringLocalizer(baseName, assembly);
            case "resx":
                return new ResxStringLocalizer(baseName, assembly);
            
            default:
                throw new NotSupportedException($"Resource type '{resourceType}' is not supported.");
        }
    }

    private string GetResourceTypeFromConfiguration(Type resourceSource)
    {
        // Configuration veya başka bir yöntemle resource tipini belirleyin
        // Örneğin, bir attribute kullanabilirsiniz veya bir configuration dosyasından okuyabilirsiniz
        // Bu örnekte, basit bir string return ediyoruz
        return "json"; // Veya "resx" veya "db"
    }
}