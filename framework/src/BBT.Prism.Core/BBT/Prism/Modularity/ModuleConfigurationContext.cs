using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Modularity;

public class ModuleConfigurationContext([NotNull] IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
    public IDictionary<string, object?> Items { get; } = new Dictionary<string, object?>();

    public object? this[string key] {
        get => Items.GetOrDefault(key);
        set => Items[key] = value;
    }
}