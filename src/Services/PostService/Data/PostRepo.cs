using PostService.Models;

namespace PostService.Data;

public class PostRepo : IPostRepo
{
    private readonly AppDbContext _context;

    public PostRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void CreatePost(Post post)
    {
        if (post == null)
        {
            throw new ArgumentNullException(nameof(post));
        }
        
        _context.Posts.Add(post);
    }

    public IEnumerable<Post> GetPostsWithFilters(int? userId = null, int? themeId = null, int? postId = null)
    {
        return _context.Posts.Where(post =>
            (userId == null || post.UserId == userId.Value) &&
            (themeId == null || post.ThemeId == themeId.Value) &&
            (postId == null || post.Id == postId.Value));
    }
}