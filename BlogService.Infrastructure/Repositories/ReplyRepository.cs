using BlogService.Core.Interfaces;
using BlogService.Domain.Models;
using BlogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Infrastructure.Repositories
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        public ReplyRepository(BlogPostContext dbContext) : base(dbContext)
        {
        }

        public async Task<Reply> GetReplyAsync(Guid replyId)
        {
            return await _dbContext.Replies
                .Include(u => u.User)
                .FirstOrDefaultAsync(p => p.Id == replyId);
        }

        public async Task<IEnumerable<Reply>> GetRepliesAsync()
        {
            return await _dbContext.Replies
                .Include(u => u.User).ToListAsync();
        }
    }
}
