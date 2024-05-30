using System;
using System.Net;
using BBT.Prism.Data;
using BBT.Prism.Domain.Entities;
using BBT.Prism.ExceptionHandling;
using BBT.Prism.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BBT.Prism.AspNetCore.ExceptionHandling;

public sealed class DefaultHttpExceptionStatusCodeFinder(IOptions<PrismExceptionHttpStatusCodeOptions> options)
    : IHttpExceptionStatusCodeFinder
{
    private PrismExceptionHttpStatusCodeOptions Options { get; } = options.Value;
    
    public HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {
        if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode &&
            exceptionWithHttpStatusCode.HttpStatusCode > 0)
        {
            return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;
        }
        
        if (exception is IHasErrorCode exceptionWithErrorCode &&
            !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
        {
            if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code!, out var status))
            {
                return status;
            }
        }
        
        if (exception is PrismValidationException)
        {
            return HttpStatusCode.BadRequest;
        }
        
        if (exception is EntityNotFoundException)
        {
            return HttpStatusCode.NotFound;
        }
        
        if (exception is PrismDbConcurrencyException)
        {
            return HttpStatusCode.Conflict;
        }

        if (exception is NotImplementedException)
        {
            return HttpStatusCode.NotImplemented;
        }

        if (exception is IBusinessException)
        {
            return HttpStatusCode.Forbidden;
        }

        return HttpStatusCode.InternalServerError;
    }
}
