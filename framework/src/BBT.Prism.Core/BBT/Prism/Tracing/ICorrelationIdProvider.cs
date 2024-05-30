using System;

namespace BBT.Prism.Tracing;

public interface ICorrelationIdProvider
{
    string? Get();

    IDisposable Change(string? correlationId);
}