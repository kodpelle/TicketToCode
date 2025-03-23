namespace TicketToCode.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = UserRoles.User; // Default role
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //M�ste ha parameterl�s konstruktor f�r EF core 
    public User() { }
   
    //Fabriksmetod f�r att skapa en anv�ndare vid registrering
    public static User Create(string name, string pwdHash, string role = UserRoles.User)
    {
        var user = new User
        {
            Username = name,
            PasswordHash = pwdHash,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
        return user;
    }
}





// Static class to define roles
public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";

}