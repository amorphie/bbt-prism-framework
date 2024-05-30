using BBT.Prism.Collections;

namespace BBT.Prism.EventBus.Domains;

public class PrismDomainEventBusOptions
{
    public ITypeList<IEventHandler> Handlers { get; } = new TypeList<IEventHandler>();
}