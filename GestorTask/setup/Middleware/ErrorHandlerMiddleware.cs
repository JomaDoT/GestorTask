using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity.Core;
using System.Reflection;
using System.ServiceModel;
using GestorTask.Utilitys.GlobalException;
using GestorTask.Utilitys.Responses;

namespace GestorTask.setup.Middleware;

// clase creada para capturar excepciones y manejarlos
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _log;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _log = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) 
        {
            _log.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message;

        switch (exception)
        {
            case BadRequestException:
                message = exception.Message;
                status = HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                message = exception.Message;
                status = HttpStatusCode.NotFound;
                break;
            case NotImplementedException:
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
                break;
            case UnauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;
            case OperationCanceledException:
                message = "Operacion fue cancelada por tiempo de espera.";
                status = HttpStatusCode.RequestTimeout;
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                break;
        }

        var exceptionResult = JsonSerializer.Serialize(new { error = ErrorHelper<string>.Response(context.Request.Path, (int)status, message) });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        await context.Response.WriteAsync(exceptionResult);
    }
}
