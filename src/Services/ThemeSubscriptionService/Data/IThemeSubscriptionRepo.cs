using ThemeSubscriptionService.Models;

namespace ThemeSubscriptionService.Data;

public interface IThemeSubscriptionRepo
{
    public void SaveChanges();
    
    void CreateThemeSubscription(ThemeSubscription themeSubscription);
    
    void RemoveThemeSubscription(ThemeSubscription themeSubscription);

    IEnumerable<ThemeSubscription> GetAllThemeSubscriptions();
    
    IEnumerable<ThemeSubscription> GetThemeSubscriptionsForUser(long userId);
    
    IEnumerable<ThemeSubscription> GetSubscriptionsByTheme(long themeId);
    
    bool IsUserSubscribedToTheme(long userId, long themeId);
}