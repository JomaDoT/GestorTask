namespace ModularPrestaMix.WebApi.Setup.Middleware;
public class SecureHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecureHeadersMiddleware(RequestDelegate request)
    {
        _next = request;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");
        context.Response.Headers.Append("Permissions-Policy", "geolocation=(self), microphone=(), camera=()");
        context.Response.Headers.Append("Feature-Policy", "geolocation 'self'; microphone 'none'; camera 'none';");
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");

        await _next(context);
    }

}