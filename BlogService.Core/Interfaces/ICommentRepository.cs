using BlogService.Domain.Models;

namespace BlogService.Core.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> GetCommentWithRepliesAsync(Guid commentId);

        Task<IEnumerable<Comment>> GetCommentsWithRepliesAsync();
    }
}
