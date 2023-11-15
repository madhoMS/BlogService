using BlogService.Core.Interfaces;
using BlogService.Domain.Models;
using BlogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BlogService.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(BlogPostContext dbContext) : base(dbContext)
        {
        }

        public async Task<Post> GetPostWithCommentsAsync(Guid postId)
        {
            return await _dbContext.Posts
                .Include(u => u.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }


        public async Task<IEnumerable<Post>> GetPostsWithCommentsAsync()
        {
            return await _dbContext.Posts
                .Include(u => u.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies).ToListAsync();
        }
    }
}
