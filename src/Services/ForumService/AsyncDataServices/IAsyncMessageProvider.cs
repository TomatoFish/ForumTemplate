using ForumService.Dtos;

namespace ForumService.AsyncDataServices;

public interface IAsyncMessageProvider
{
    void PublishPostCreated(PostCreatePublishDto postCreatePublish);
}