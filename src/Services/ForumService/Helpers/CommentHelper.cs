using ForumService.Models;

namespace ForumService.Helpers;

public static class CommentHelper
{
    public static List<Comment> FormatStructure(IEnumerable<Comment> initialComments, long? parentId)
    {
        var tree = new List<Comment>();

        foreach (var comment in initialComments)
        {
            if (comment.ParentCommentId == parentId)
            {
                tree.Add(comment);
            }
        }
        
        return tree;
    }
}