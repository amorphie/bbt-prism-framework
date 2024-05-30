using System;

namespace BBT.Prism.EventBus.Domains;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class DomainEventHandlerOrderAttribute(int order) : Attribute
{
    /// <summary>
    /// Handlers execute in ascending numeric value of the Order property.
    /// </summary>
    public int Order { get; set; } = order;
}