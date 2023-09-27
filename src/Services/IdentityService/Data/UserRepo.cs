using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Data;

public class UserRepo : IUserRepo
{
    // private readonly UserManager<User> _userManager;
    //
    // public UserRepo(UserManager<User> userManager)
    // {
    //     _userManager = userManager;
    // }
    //
    // public void CreateUser(User user)
    // {
    //     _userManager.CreateAsync(user);
    // }
    //
    // public IEnumerable<User> GetAllUsers()
    // {
    //     return _userManager.Users;
    // }
    //
    // public IEnumerable<User> GetUsersByRole(string role)
    // {
    //     return _userManager.Users.Where(user => user.Role == role);
    // }
    //
    // public User? GetUserById(string id)
    // {
    //     return _userManager.Users.FirstOrDefault(user => user.Id == id);
    // }
    //
    // public User? GetUserByEmail(string email)
    // {
    //     return _userManager.Users.FirstOrDefault(user => user.Email == email);
    // }
    //
    // public User? GetUserByUsername(string username)
    // {
    //     return _userManager.Users.FirstOrDefault(user => user.UserName == username);
    // }
    
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