using campus.DBContext;
using Microsoft.AspNetCore.Authorization;

namespace campus.AdditionalServices.TokenHelpers;

public class BlackTokenHandler : AuthorizationHandler<BlackListRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public BlackTokenHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BlackListRequirement requirement)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RedisRepository>();
            
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            
        var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length);
                
            var blackToken = await db.IsBlacklisted(token);

            if (blackToken)
            {
                httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }
        }
        else
        {
            httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Fail();
        }
    }
}