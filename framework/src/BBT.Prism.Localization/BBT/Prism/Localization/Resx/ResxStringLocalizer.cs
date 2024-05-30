using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Localization;

namespace BBT.Prism.Localization.Resx;

public class ResxStringLocalizer : IStringLocalizer
{
    private readonly ResourceManager _resourceManager;

    public ResxStringLocalizer(string baseName, Assembly assembly)
    {
        _resourceManager = new ResourceManager(baseName, assembly);
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = _resourceManager.GetString(name);
            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = this[name];
            var value = string.Format(format, arguments);
            return new LocalizedString(name, value, format.ResourceNotFound);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var resourceSet = _resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        return resourceSet!.Cast<DictionaryEntry>().Select(entry => new LocalizedString(entry.Key.ToString()!, entry.Value!.ToString()!));
    }
}