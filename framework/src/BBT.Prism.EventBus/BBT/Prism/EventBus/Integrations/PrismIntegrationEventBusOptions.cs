using BBT.Prism.Collections;

namespace BBT.Prism.EventBus.Integrations;

public class PrismIntegrationEventBusOptions
{
    public ITypeList<IEventHandler> Handlers { get; } = new TypeList<IEventHandler>();
}