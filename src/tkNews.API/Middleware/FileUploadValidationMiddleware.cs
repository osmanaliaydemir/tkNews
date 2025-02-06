using Microsoft.AspNetCore.Http;

namespace tkNews.API.Middleware;

public class FileUploadValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

    public FileUploadValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.HasFormContentType && context.Request.Form.Files.Any())
        {
            foreach (var file in context.Request.Form.Files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_allowedExtensions.Contains(extension))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = $"File type {extension} is not allowed." });
                    return;
                }

                if (file.Length > _maxFileSize)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = $"File size exceeds the limit of 5MB." });
                    return;
                }
            }
        }

        await _next(context);
    }
}

public static class FileUploadValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseFileUploadValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FileUploadValidationMiddleware>();
    }
} 