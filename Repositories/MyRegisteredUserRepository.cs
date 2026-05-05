using RolesDemo.Data;
using RolesDemo.Models;

namespace RolesDemo.Repositories;

public class MyRegisteredUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MyRegisteredUserRepository> _logger;

    public MyRegisteredUserRepository(
        ApplicationDbContext context,
        ILogger<MyRegisteredUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

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
