using System.Text.Json;
using ThemeSubscriptionService.Data;
using ThemeSubscriptionService.Dtos;

namespace ThemeSubscriptionService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PostCreated:
                ProcessPostCreation(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string stringNotificationMessage)
    {
        Console.WriteLine("--> Defining event");

        var eventType = JsonSerializer.Deserialize<PostCreatePublishDto>(stringNotificationMessage);
        switch (eventType?.Event)
        {
            case "post_created":
                Console.WriteLine("--> Defined post_created event");
                return EventType.PostCreated;
            default:
                Console.WriteLine("--> Undefined event");
                return EventType.Undefined;
        }
    }

    private void ProcessPostCreation(string postCreatePublishedMessage)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IThemeSubscriptionRepo>();
            var postCreatePublish = JsonSerializer.Deserialize<PostCreatePublishDto>(postCreatePublishedMessage);
            var subscriptions = repository.GetSubscriptionsByTheme(postCreatePublish.ThemeId);
            Console.WriteLine($"--> Processing post creation in theme: {postCreatePublish.ThemeId}");
            foreach (var subscription in subscriptions)
            {
                if (subscription.UserId == postCreatePublish.UserId) continue;

                Console.WriteLine($"--> Alarm to user {postCreatePublish.UserId}");
                // todo: create alarm
            }
        }
    }

    private enum EventType
    {
        Undefined,
        PostCreated
    }
}