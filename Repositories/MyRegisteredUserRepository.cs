using RolesDemo.Data;
using RolesDemo.Models;

namespace RolesDemo.Repositories;

public class MyRegisteredUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MyRegisteredUserRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyRegisteredUserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">The logger instance.</param>
    public MyRegisteredUserRepository(
        ApplicationDbContext context,
        ILogger<MyRegisteredUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Adds a new registered user record to the database.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name (optional).</param>
    public void AddRegisteredUser(string email,
        string firstName,
        string? lastName = null)
    {
        try
        {
            var user = new MyRegisteredUser
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName ?? string.Empty
            };

            _context.MyRegisteredUsers.Add(user);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Error adding registered user. " +
                $"Email: {email}, FirstName: {firstName}");

            Console.WriteLine($"Error adding registered user: {ex.Message}");
        }
    }
}
