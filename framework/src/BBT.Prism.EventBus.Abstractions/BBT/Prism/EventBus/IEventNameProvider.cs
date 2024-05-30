using System;

namespace BBT.Prism.EventBus;

public interface IEventNameProvider
{
    string GetName(Type eventType);
}
