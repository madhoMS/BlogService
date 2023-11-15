using BlogService.Application.DTOs;
using BlogService.Common.Utilities;

namespace BlogService.Core.Interfaces
{
    public interface IReplyService
    {
        Task<ResponseWrapper> ReplyCreateAsync(CreateReplyDto createReply);
        Task<ResponseWrapper> ReplyUpdateAsync(Guid replyId, CreateReplyDto updateReply);
        Task<ResponseWrapper> GetReplyAsync(Guid replyId);
        Task<ResponseWrapper> GetAllReplyAsync();
        Task<ResponseWrapper> DeleteReplyAsync(Guid replyId);
    }
}
