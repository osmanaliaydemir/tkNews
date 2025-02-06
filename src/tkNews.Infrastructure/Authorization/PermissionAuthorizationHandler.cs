using Microsoft.AspNetCore.Authorization;
using tkNews.Application.Common.Interfaces;
using tkNews.Domain.Entities.Identity;
using tkNews.Domain.Enums;

namespace tkNews.Infrastructure.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permissions Permission { get; }

    public PermissionRequirement(Permissions permission)
    {
        Permission = permission;
    }
}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;
    
    public PermissionAuthorizationHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User == null)
        {
            return;
        }
        
        var user = context.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            return;
        }
        
        var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return;
        }
        
        // Burada ApplicationUser'ı almak için bir servis kullanılmalı
        // Şimdilik mock bir user oluşturuyoruz
        var applicationUser = new ApplicationUser { Id = userId };
        
        if (await _permissionService.HasPermissionAsync(applicationUser, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
} 