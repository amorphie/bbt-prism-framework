namespace BBT.Prism.EventBus;

public class EventHandlerInvokerCacheItem
{
    public IEventHandlerMethodExecutor? Domain { get; set; }

    public IEventHandlerMethodExecutor? Integration { get; set; }
}