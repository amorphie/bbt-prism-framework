using System;
using System.Threading.Tasks;
using BBT.Prism.EventBus.Domains;
using BBT.Prism.Guids;
using BBT.Prism.Timing;
using BBT.Prism.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Prism.EventBus.Integrations;

public abstract class IntegrationEventBusBase(
    IServiceScopeFactory serviceScopeFactory,
    IGuidGenerator guidGenerator,
    IClock clock,
    IEventHandlerInvoker eventHandlerInvoker,
    IDomainEventBus domainEventBus,
    ICorrelationIdProvider correlationIdProvider,
    IOptions<PrismIntegrationEventBusOptions> integrationEventBusOptions
    )
    : EventBusBase(serviceScopeFactory,
        eventHandlerInvoker), IIntegrationEventBus
{
    
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected IClock Clock { get; } = clock;
    protected IDomainEventBus DomainEventBus { get; } = domainEventBus;
    protected ICorrelationIdProvider CorrelationIdProvider { get; } = correlationIdProvider;
    protected PrismIntegrationEventBusOptions IntegrationEventBusOptions { get; } = integrationEventBusOptions.Value;

    public IDisposable Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    public async override Task PublishAsync(Type eventType, object eventData)
    {
        await TriggerIntegrationEventSentAsync(new IntegrationEventSent()
        {
            Source = IntegrationEventSource.Direct,
            EventName = EventNameAttribute.GetNameOrDefault(eventType),
            EventData = eventData
        });

        await PublishToEventBusAsync(eventType, eventData);
    }
    
    protected abstract byte[] Serialize(object eventData);

    protected virtual async Task TriggerHandlersDirectAsync(Type eventType, object eventData)
    {
        await TriggerIntegrationEventReceivedAsync(new IntegrationEventReceived
        {
            Source = IntegrationEventSource.Direct,
            EventName = EventNameAttribute.GetNameOrDefault(eventType),
            EventData = eventData
        });

        await TriggerHandlersAsync(eventType, eventData);
    }

    protected virtual async Task TriggerIntegrationEventSentAsync(IntegrationEventSent distributedEvent)
    {
        try
        {
            await DomainEventBus.PublishAsync(distributedEvent);
        }
        catch
        {
            // ignored
        }
    }

    protected virtual async Task TriggerIntegrationEventReceivedAsync(IntegrationEventReceived distributedEvent)
    {
        try
        {
            await DomainEventBus.PublishAsync(distributedEvent);
        }
        catch 
        {
            // ignored
        }
    }
}
