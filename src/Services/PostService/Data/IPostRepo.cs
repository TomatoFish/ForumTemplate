using PostService.Models;

namespace PostService.Data;

public interface IPostRepo
{
   void SaveChanges();
   
   void CreatePost(Post post);

   IEnumerable<Post> GetPostsWithFilters(int? userId = null, int? themeId = null, int? postId = null);
}