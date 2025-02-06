using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using tkNews.Application.Common.Models;

namespace tkNews.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ProblemDetails();

        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Title = "Validation Error";
                response.Detail = JsonSerializer.Serialize(validationEx.Errors);
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Status = (int)HttpStatusCode.Unauthorized;
                response.Title = "Unauthorized";
                response.Detail = "You are not authorized to access this resource.";
                response.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                break;

            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Status = (int)HttpStatusCode.NotFound;
                response.Title = "Not Found";
                response.Detail = "The requested resource was not found.";
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Title = "Server Error";
                response.Detail = _env.IsDevelopment() ? exception.ToString() : "An internal server error has occurred.";
                response.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                break;
        }

        var result = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(result);
    }
} 