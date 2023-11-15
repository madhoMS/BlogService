using BlogService.Core.Interfaces;
using BlogService.Domain.Models;
using BlogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogPostContext dbContext) : base(dbContext)
        {
        }

        public async Task<Comment> GetCommentWithRepliesAsync(Guid commentId)
        {
            return await _dbContext.Comments
                .Include(u => u.User)
                  .Include(c => c.Replies)
                .FirstOrDefaultAsync(p => p.Id == commentId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsWithRepliesAsync()
        {
            return await _dbContext.Comments
                .Include(u => u.User)
                  .Include(c => c.Replies).ToListAsync();
        }
    }
}
