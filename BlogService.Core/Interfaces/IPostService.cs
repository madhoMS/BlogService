using BlogService.Application.DTOs;
using BlogService.Common.Utilities;

namespace BlogService.Core.Interfaces
{
    public interface IPostService
    {
        Task<ResponseWrapper> PostCreateAsync(CreatePostDto createPost);
        Task<ResponseWrapper> PostUpdateAsync(Guid postId, CreatePostDto updatePost);
        Task<ResponseWrapper> GetPostAsync(Guid postId);
        Task<ResponseWrapper> GetAllPostAsync();
        Task<ResponseWrapper> DeletePostAsync(Guid postId);
        Task<ResponseWrapper> GetPostWithCommentsAsync(Guid postId);
        Task<ResponseWrapper> GetAllPostWithCommentAndRepliesAsync();
    }
}
