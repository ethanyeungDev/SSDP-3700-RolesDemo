using Microsoft.AspNetCore.Identity;
using RolesDemo.ViewModels;

namespace RolesDemo.Repositories;

public class UserRoleRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleRepository"/> class.
    /// </summary>
    /// <param name="userManager">The ASP.NET Core Identity user manager.</param>
    public UserRoleRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Assigns a role to the user with the specified email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="roleName">The name of the role to assign.</param>
    /// <returns><c>true</c> if the role was assigned successfully; otherwise <c>false</c>.</returns>
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

    /// <summary>
    /// Removes a role from the user with the specified email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="roleName">The name of the role to remove.</param>
    /// <returns><c>true</c> if the role was removed successfully; otherwise <c>false</c>.</returns>
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

    /// <summary>
    /// Gets all roles assigned to the user with the specified email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>A list of role names assigned to the user, or an empty list if the user is not found.</returns>
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
