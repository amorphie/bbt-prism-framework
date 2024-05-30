using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace BBT.Prism.Localization.Json;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly Dictionary<string, string> _localizations;

    public JsonStringLocalizer(string baseName, Assembly assembly)
    {
        var resourceStream = assembly.GetManifestResourceStream(baseName);
        using (var reader = new StreamReader(resourceStream!))
        {
            var json = reader.ReadToEnd();
            _localizations = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
        }
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = _localizations.ContainsKey(name) ? _localizations[name] : name;
            return new LocalizedString(name, value);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = this[name];
            var value = string.Format(format, arguments);
            return new LocalizedString(name, value);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return _localizations.Select(l => new LocalizedString(l.Key, l.Value));
    }
}