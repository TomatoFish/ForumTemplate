using ForumService.Models;

namespace ForumService.Data;

public interface ICommentRepo
{
    void SaveChanges();
   
    void CreateComment(Comment post);

    IEnumerable<Comment> GetCommentsWithFilters(int? postId, int? userId, int? parentCommentId, int? commentId);
}