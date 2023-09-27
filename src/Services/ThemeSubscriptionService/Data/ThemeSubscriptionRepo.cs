using ThemeSubscriptionService.Models;

namespace ThemeSubscriptionService.Data;

public class ThemeSubscriptionRepo : IThemeSubscriptionRepo
{
    private readonly AppDbContext _context;


    public ThemeSubscriptionRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void CreateThemeSubscription(ThemeSubscription themeSubscription)
    {
        _context.ThemeSubscriptions.Add(themeSubscription);
    }

    public void RemoveThemeSubscription(ThemeSubscription themeSubscription)
    {
        _context.ThemeSubscriptions.Remove(themeSubscription);
    }

    public IEnumerable<ThemeSubscription> GetAllThemeSubscriptions()
    {
        return _context.ThemeSubscriptions;
    }

    public IEnumerable<ThemeSubscription> GetThemeSubscriptionsForUser(long userId)
    {
        return _context.ThemeSubscriptions.Where(ts => ts.UserId == userId);
    }

    public IEnumerable<ThemeSubscription> GetSubscriptionsByTheme(long themeId)
    {
        return _context.ThemeSubscriptions.Where(ts => ts.ThemeId == themeId);
    }

    public bool IsUserSubscribedToTheme(long userId, long themeId)
    {
        return _context.ThemeSubscriptions.Any(ts => ts.UserId == userId && ts.ThemeId == themeId);
    }
}