using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using tkNews.Application.Common.Interfaces;
using tkNews.Domain.Entities.Identity;
using tkNews.Domain.Enums;
using tkNews.Infrastructure.Data.Identity;

namespace tkNews.Infrastructure.Services;

public class PermissionService : IPermissionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationIdentityDbContext _context;

    public PermissionService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationIdentityDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(ApplicationUser user, Permissions permission)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = Permissions.None;

        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var rolePermissions = await GetRolePermissionsAsync(role);
                foreach (var rolePermission in rolePermissions)
                {
                    permissions |= rolePermission;
                }
            }
        }

        return permissions.HasFlag(permission);
    }

    public async Task<IList<Permissions>> GetUserPermissionsAsync(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new HashSet<Permissions>();

        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var rolePermissions = await GetRolePermissionsAsync(role);
                foreach (var permission in rolePermissions)
                {
                    permissions.Add(permission);
                }
            }
        }

        return permissions.ToList();
    }

    public async Task<bool> GrantPermissionToRoleAsync(ApplicationRole role, Permissions permission)
    {
        var currentPermissions = await GetRolePermissionsAsync(role);
        var newPermissions = currentPermissions.ToHashSet();
        newPermissions.Add(permission);

        var claim = new System.Security.Claims.Claim("Permissions", permission.ToString());
        var result = await _roleManager.AddClaimAsync(role, claim);

        return result.Succeeded;
    }

    public async Task<bool> RevokePermissionFromRoleAsync(ApplicationRole role, Permissions permission)
    {
        var claim = new System.Security.Claims.Claim("Permissions", permission.ToString());
        var result = await _roleManager.RemoveClaimAsync(role, claim);

        return result.Succeeded;
    }

    public async Task<IList<Permissions>> GetRolePermissionsAsync(ApplicationRole role)
    {
        var claims = await _roleManager.GetClaimsAsync(role);
        var permissions = new HashSet<Permissions>();

        foreach (var claim in claims.Where(c => c.Type == "Permissions"))
        {
            if (Enum.TryParse<Permissions>(claim.Value, out var permission))
            {
                permissions.Add(permission);
            }
        }

        return permissions.ToList();
    }
} 