using System.Collections.Generic;
using System.Net;

namespace BBT.Prism.AspNetCore.ExceptionHandling;

public class PrismExceptionHttpStatusCodeOptions
{
    public IDictionary<string, HttpStatusCode> ErrorCodeToHttpStatusCodeMappings { get; } = new Dictionary<string, HttpStatusCode>();

    public void Map(string errorCode, HttpStatusCode httpStatusCode)
    {
        ErrorCodeToHttpStatusCodeMappings[errorCode] = httpStatusCode;
    }
}