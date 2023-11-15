using BlogService.Domain.Models;

namespace BlogService.Core.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> GetPostWithCommentsAsync(Guid postId);
        Task<IEnumerable<Post>> GetPostsWithCommentsAsync();
    }
}
