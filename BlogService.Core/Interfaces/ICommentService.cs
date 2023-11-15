using BlogService.Application.DTOs;
using BlogService.Common.Utilities;

namespace BlogService.Core.Interfaces
{
    public interface ICommentService
    {
        Task<ResponseWrapper> CommentCreateAsync(CreateCommentDto createComment);
        Task<ResponseWrapper> CommentUpdateAsync(Guid commentId, CreateCommentDto updateComment);
        Task<ResponseWrapper> GetCommentAsync(Guid commentId);
        Task<ResponseWrapper> GetAllCommentAsync();
        Task<ResponseWrapper> DeleteCommentAsync(Guid commentId);
        Task<ResponseWrapper> GetCommentWithRepliesAsync(Guid commentId);
        Task<ResponseWrapper> GetCommentsWithRepliesAsync();
    }
}
