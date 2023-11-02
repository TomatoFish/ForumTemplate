using IdentityService.Models;

namespace IdentityService.Data;

public class UserRepo : IUserRepo
{
    private readonly AppDbContext _dbContext;
    
    public UserRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
    
    public void CreateUser(User user)
    {
        _dbContext.Add(user);
    }
    
    public void UpdateUser(User user)
    {
        _dbContext.Update(user);
    }
    
    public IEnumerable<User> GetAllUsers()
    {
        return _dbContext.Users;
    }
    
    public IEnumerable<User> GetUsersByRole(string role)
    {
        return _dbContext.Users.Where(user => user.Role == role);
    }
    
    public User? GetUserById(long id)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Id == id);
    }
    
    public User? GetUserByEmail(string email)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Email == email);
    }
    
    public User? GetUserByUsername(string username)
    {
        return _dbContext.Users.FirstOrDefault(user => user.UserName == username);
    }
}