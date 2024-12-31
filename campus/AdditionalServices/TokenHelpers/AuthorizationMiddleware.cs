using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace campus.AdditionalServices.TokenHelpers;

public class AuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult is { Forbidden: true, Challenged: false })
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}