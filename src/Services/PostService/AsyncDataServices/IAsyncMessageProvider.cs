using PostService.Dtos;

namespace PostService.AsyncDataServices;

public interface IAsyncMessageProvider
{
    void PublishPostCreated(PostCreatePublishDto postCreatePublish);
}