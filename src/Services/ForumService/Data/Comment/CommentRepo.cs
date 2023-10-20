using ForumService.Models;

namespace ForumService.Data;

public class CommentRepo : ICommentRepo
{
    private readonly AppDbContext _context;

    public CommentRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void CreateComment(Comment comment)
    {
        if (comment == null)
        {
            throw new ArgumentNullException(nameof(comment));
        }
        
        _context.Comments.Add(comment);
    }

    public IEnumerable<Comment> GetCommentsWithFilters(int? postId = null, int? userId = null, int? parentCommentId = null, int? commentId = null)
    {
        return _context.Comments.Where(comment =>
            (postId == null || (comment.Post != null && comment.Post.Id == postId.Value)) &&
            (userId == null || comment.UserId == userId.Value) &&
            (parentCommentId == null || (parentCommentId.Value == 0 && comment.ParentComment == null) || (comment.ParentComment != null && comment.ParentComment.Id == parentCommentId.Value)) &&
            (commentId == null || comment.Id == commentId.Value));
    }
}