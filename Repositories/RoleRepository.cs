using Microsoft.AspNetCore.Identity;
using RolesDemo.Data;
using RolesDemo.ViewModels;

namespace RolesDemo.Repositories;

public class RoleRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleRepository"/> class and ensures the initial Admin role exists if it does not already exist.
    /// </summary>
    /// <param name="context">The database context.</param>
    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
        CreateInitialRole();
    }

    /// <summary>
    /// Returns a list of all roles as view model records.
    /// </summary>
    /// <returns>A list of <see cref="RoleVM"/> representing all roles.</returns>
    public List<RoleVM> GetAllRoles()
    {
        return _context.Roles
            .Select(r => new RoleVM
            {
                Id = r.Id,
                RoleName = r.Name
            }).ToList();
    }

    /// <summary>
    /// Returns a single role view model matching the given role ID.
    /// </summary>
    /// <param name="id">The ID of the role to retrieve.</param>
    /// <returns>A <see cref="RoleVM"/> if found; otherwise <c>null</c>.</returns>
    public RoleVM? GetRoleById(string id)
    {
        var role = _context.Roles.FirstOrDefault(r => r.Id == id);

        if (role != null)
        {
            return new RoleVM
            {
                Id = role.Id,
                RoleName = role.Name
            };
        }

        return null;
    }

    /// <summary>
    /// Determines whether the role with the given ID is currently assigned to any users.
    /// </summary>
    /// <param name="roleId">The role ID to check.</param>
    /// <returns><c>true</c> if the role is assigned to at least one user; otherwise <c>false</c>.</returns>
    public bool IsRoleAssigned(string roleId)
    {
        return _context.UserRoles.Any(ur => ur.RoleId == roleId);
    }

    /// <summary>
    /// Deletes the role with the given ID, provided it is not assigned to any users.
    /// </summary>
    /// <param name="roleId">The ID of the role to delete.</param>
    /// <returns><c>true</c> if deleted successfully; <c>false</c> if the role is assigned to users or not found.</returns>
    public bool DeleteRole(string roleId)
    {
        try
        {
            if (IsRoleAssigned(roleId))
            {
                return false;
            }

            var role = _context.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                return false;
            }

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting role: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Returns a single role view model matching the given role name.
    /// </summary>
    /// <param name="roleName">The name of the role to retrieve.</param>
    /// <returns>A <see cref="RoleVM"/> if found; otherwise <c>null</c>.</returns>
    public RoleVM? GetRole(string roleName)
    {
        var role = _context.Roles
            .FirstOrDefault(r => r.Name == roleName);

        if (role != null)
        {
            return new RoleVM
            {
                Id = role.Id,
                RoleName = role.Name
            };
        }

        return null;
    }

    /// <summary>
    /// Creates a new role record with the given name.
    /// </summary>
    /// <param name="roleName">The name of the role to create.</param>
    /// <returns><c>true</c> if the role was created successfully; otherwise <c>false</c>.</returns>
    public bool CreateRole(string roleName)
    {
        try
        {
            _context.Roles.Add(new IdentityRole
            {
                Id = roleName,
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });

            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating role:" +
                              $" {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Creates the initial Admin role if it does not already exist.
    /// </summary>
    public void CreateInitialRole()
    {
        const string ADMIN = "Admin";

        if (GetRole(ADMIN) == null)
        {
            CreateRole(ADMIN);
        }
    }
}
