using Microsoft.Extensions.Logging;

namespace BBT.Prism.Logging;

public interface IExceptionWithSelfLogging
{
    void Log(ILogger logger);
}