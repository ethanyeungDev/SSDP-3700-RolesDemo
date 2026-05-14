using RolesDemo.Data;
using RolesDemo.ViewModels;

namespace RolesDemo.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns a list of all registered users as view model records.
    /// </summary>
    /// <returns>A list of <see cref="UserVM"/> containing each user's email address.</returns>
    public List<UserVM> GetAllUsers()
    {
        return _context.Users
            .Select(u => new UserVM { Email = u.Email ?? string.Empty })
            .ToList();
    }
}
