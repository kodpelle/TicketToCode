using TicketToCode.Core.Data;
using TicketToCode.Core.Models;

namespace TicketToCode.Core.Services;

public interface IAuthService
{
    User? Login(string username, string password);
    User? Register(string username, string password);
}

// TODO: Implement better auth
/// <summary>
/// Simple auth service to enable registering and login in, should be replaced before release
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDBContext _applicationDBcontext;

    public AuthService(ApplicationDBContext applicationDBContext)
    {
        _applicationDBcontext = applicationDBContext;
    }

    public User? Login(string username, string password)
    {
        var user = _applicationDBcontext.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }

    public User? Register(string username, string password)
    {
        if (_applicationDBcontext.Users.Any(u => u.Username == username))
        {
            return null;
        }
        string role;
        if (username.Contains("Admin",StringComparison.OrdinalIgnoreCase))
        {
            role = UserRoles.Admin;
        }
        else
        {
            role = UserRoles.User;
        }

        var user = User.Create(username, BCrypt.Net.BCrypt.HashPassword(password),role);

        _applicationDBcontext.Users.Add(user);
        //save user to the database
        _applicationDBcontext.SaveChanges();

        return user;
    }
} 