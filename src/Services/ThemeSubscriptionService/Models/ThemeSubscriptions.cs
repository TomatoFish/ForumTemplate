using System.ComponentModel.DataAnnotations;

namespace ThemeSubscriptionService.Models;

public class ThemeSubscription
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public long ThemeId { get; set; }
}