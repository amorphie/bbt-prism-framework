using System;
using System.Threading;

namespace BBT.Prism.Tracing;

public sealed class DefaultCorrelationIdProvider : ICorrelationIdProvider
{
    private readonly AsyncLocal<string?> _currentCorrelationId = new AsyncLocal<string?>();

    private string? CorrelationId => _currentCorrelationId.Value;

    public string? Get()
    {
        return CorrelationId;
    }

    public IDisposable Change(string? correlationId)
    {
        var parent = CorrelationId;
        _currentCorrelationId.Value = correlationId;
        return new DisposeAction(() =>
        {
            _currentCorrelationId.Value = parent;
        });
    }
}