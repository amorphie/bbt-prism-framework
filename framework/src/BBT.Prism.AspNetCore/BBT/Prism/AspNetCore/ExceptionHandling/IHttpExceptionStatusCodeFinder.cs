using System;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace BBT.Prism.AspNetCore.ExceptionHandling;

public interface IHttpExceptionStatusCodeFinder
{
    HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception);
}