using Microsoft.AspNetCore.Identity;
using RolesDemo.Data;
using RolesDemo.ViewModels;

namespace RolesDemo.Repositories;

public class RoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
        CreateInitialRole();
    }

    // Return a list of RoleVM records
    public List<RoleVM> GetAllRoles()
    {
        return _context.Roles
            .Select(r => new RoleVM
            {
                Id = r.Id,
                RoleName = r.Name
            }).ToList();
    }

    // Return a single RoleVM record.
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

    // Create a new role record.    
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

    // Create initial role.
    public void CreateInitialRole()
    {
        const string ADMIN = "Admin";

        if (GetRole(ADMIN) == null)
        {
            CreateRole(ADMIN);
        }
    }
}