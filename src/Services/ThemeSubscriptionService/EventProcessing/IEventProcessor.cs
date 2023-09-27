namespace ThemeSubscriptionService.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}