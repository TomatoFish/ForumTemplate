using IdentityService.Models;

namespace IdentityService.Data;

public interface IUserRepo
{
    void SaveChanges();

    void CreateUser(User user);
    
    IEnumerable<User> GetAllUsers();
    
    IEnumerable<User> GetUsersByRole(string role);

    User? GetUserById(long id);
    
    User? GetUserByEmail(string email);
    
    User? GetUserByUsername(string username);
}