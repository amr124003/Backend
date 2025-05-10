using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging; // Added for potential logging
using myapp.Services; // Placeholder: Adjust if your services are in a different namespace


namespace myapp.middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private bool RequiresAuthentication(PathString path)
    {
        // Define paths that require authentication.
        // This is a simple example, you might want a more sophisticated approach
        // based on attributes on controllers or a configuration file.
        if (path.StartsWithSegments("/api/secure"))
        {
            return true;
        }
        return false;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request path requires authentication by calling the RequiresAuthentication method.
        if (RequiresAuthentication(context.Request.Path))
        {
            // Only attempt to extract and validate the token if authentication is required for the path.
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                // Use the injected ITokenService to validate the token
                var tokenService = context.RequestServices.GetRequiredService<ITokenService>(); // Resolve the service per request
                                                                                                // ValidateToken should return a ClaimsPrincipal if valid, or null if invalid/expired
                var principal = tokenService.ValidateToken(token);

                // If the token is valid and a ClaimsPrincipal is returned, set HttpContext.User
                if (principal != null)
                {
                    context.User = principal; // Set the authenticated user principal
                }
            }
        }

        await _next(context);
    }
}

// Extension method to easily add the middleware
public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}