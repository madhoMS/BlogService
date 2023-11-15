using BlogService.Domain.Models;

namespace BlogService.Core.Interfaces
{
    public interface IReplyRepository : IRepository<Reply>
    {
        Task<Reply> GetReplyAsync(Guid replyId);

        Task<IEnumerable<Reply>> GetRepliesAsync();
    }
}
