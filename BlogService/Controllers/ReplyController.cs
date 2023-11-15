using BlogService.Application.DTOs;
using BlogService.Common.Utilities;
using BlogService.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReplyController : ControllerBase
    {
        private readonly IReplyService _replyService;

        public ReplyController(IReplyService replyService)
        {
            this._replyService = replyService;
        }

        [HttpPost]
        public async Task<ResponseWrapper> ReplyCreate(CreateReplyDto createReply)
        {
            return await _replyService.ReplyCreateAsync(createReply);
        }

        [HttpPut]
        public async Task<ResponseWrapper> ReplyUpdate(Guid replyId, CreateReplyDto updateReply)
        {
            return await _replyService.ReplyUpdateAsync(replyId, updateReply);
        }

        [HttpGet("{replyId}")]
        public async Task<ResponseWrapper> GetReply(Guid replyId)
        {
            return await _replyService.GetReplyAsync(replyId);
        }

        [HttpGet]
        public async Task<ResponseWrapper> GetAllReply()
        {
            return await _replyService.GetAllReplyAsync();
        }

        [HttpDelete]
        public async Task<ResponseWrapper> DeleteReply(Guid replyId)
        {
            return await _replyService.DeleteReplyAsync(replyId);
        }
    }
}
