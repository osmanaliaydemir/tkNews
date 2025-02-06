using tkNews.Domain.Entities.Identity;
using tkNews.Domain.Enums;

namespace tkNews.Application.Common.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(ApplicationUser user, Permissions permission);
    Task<IList<Permissions>> GetUserPermissionsAsync(ApplicationUser user);
    Task<bool> GrantPermissionToRoleAsync(ApplicationRole role, Permissions permission);
    Task<bool> RevokePermissionFromRoleAsync(ApplicationRole role, Permissions permission);
    Task<IList<Permissions>> GetRolePermissionsAsync(ApplicationRole role);
} 