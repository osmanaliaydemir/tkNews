using Microsoft.AspNetCore.Identity;
using tkNews.Domain.Entities.Identity;
using tkNews.Domain.Enums;

namespace tkNews.Infrastructure.Data.Identity;

public class IdentityDataSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityDataSeeder(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        await CreateRolesAsync();
        await CreateAdminUserAsync();
    }

    private async Task CreateRolesAsync()
    {
        // Create roles if they don't exist
        var roles = new[]
        {
            new { Name = "Admin", Description = "System administrator with full access", Permissions = Permissions.Admin },
            new { Name = "Editor", Description = "Content editor with article management access", Permissions = Permissions.Editor },
            new { Name = "Author", Description = "Content author with limited article access", Permissions = Permissions.Author },
            new { Name = "User", Description = "Regular user with basic access", Permissions = Permissions.User }
        };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                var applicationRole = new ApplicationRole(role.Name)
                {
                    Description = role.Description
                };

                var result = await _roleManager.CreateAsync(applicationRole);
                if (result.Succeeded)
                {
                    // Add permissions to role
                    await _roleManager.AddClaimAsync(applicationRole, 
                        new System.Security.Claims.Claim("Permissions", role.Permissions.ToString()));
                }
            }
        }
    }

    private async Task CreateAdminUserAsync()
    {
        // Create admin user if it doesn't exist
        var adminEmail = "admin@teknolojikafasi.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
} 