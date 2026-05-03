using RolesDemo.Data;
using RolesDemo.ViewModels;

namespace RolesDemo.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Return a list of UserVM records with email addresses.
    public List<UserVM> GetAllUsers()
    {
        return _context.Users
            .Select(u => new UserVM { Email = u.Email ?? string.Empty })
            .ToList();
    }
}
