using Microsoft.AspNetCore.Identity;
using RolesDemo.ViewModels;

namespace RolesDemo.Repositories;

public class UserRoleRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserRoleRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // Assign a role to a user.
    public async Task<bool> AddUserRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        return false;
    }

    // Remove role from a user.
    public async Task<bool> RemoveUserRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        return false;
    }

    // Get all roles of a specific user.
    public async Task<IList<string>> GetUserRolesAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return await _userManager.GetRolesAsync(user);
        }

        return new List<string>();
    }
}
