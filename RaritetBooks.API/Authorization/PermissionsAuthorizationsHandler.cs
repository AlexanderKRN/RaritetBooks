using Microsoft.AspNetCore.Authorization;
using RaritetBooks.Infrastructure.Constants;

namespace RaritetBooks.API.Authorization;

public class PermissionsAuthorizationsHandler : AuthorizationHandler<HasPermissionAttribute>
{
    private readonly ILogger<PermissionsAuthorizationsHandler> _logger;

    public PermissionsAuthorizationsHandler(ILogger<PermissionsAuthorizationsHandler> logger)
    {
        _logger = logger;
    }
    
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        HasPermissionAttribute requirement)
    {
        var permissions = context.User.Claims
            .Where(c => c.Type == AuthConstants.PERMISSION)
            .Select(c => c.Value);

        if (!permissions.Contains(requirement.Permission))
        {
            _logger.LogInformation("User has no permission: {permission}", requirement.Permission);
            return Task.CompletedTask;
        }

        _logger.LogInformation("User has permission: {permission}", requirement.Permission);
        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}