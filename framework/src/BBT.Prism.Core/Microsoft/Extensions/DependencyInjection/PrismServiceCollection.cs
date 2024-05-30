using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection;

public class PrismServiceCollection(IServiceCollection services) : IServiceCollection
{
    public Action<ServiceDescriptor>? OnRegisteredCallback { get; set; }

    public ServiceDescriptor this[int index] 
    {
        get => services[index];
        set => services[index] = value;
    }

    public int Count => services.Count;

    public bool IsReadOnly => services.IsReadOnly;

    public void Add(ServiceDescriptor item)
    {
        services.Add(item);
        OnRegisteredCallback?.Invoke(item);
    }

    public void Clear() => services.Clear();

    public bool Contains(ServiceDescriptor item) => services.Contains(item);

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => services.CopyTo(array, arrayIndex);

    public IEnumerator<ServiceDescriptor> GetEnumerator() => services.GetEnumerator();

    public int IndexOf(ServiceDescriptor item) => services.IndexOf(item);

    public void Insert(int index, ServiceDescriptor item) => services.Insert(index, item);

    public bool Remove(ServiceDescriptor item) => services.Remove(item);

    public void RemoveAt(int index) => services.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => services.GetEnumerator();
}