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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            this._commentService = commentService;
        }

        [HttpPost]
        public async Task<ResponseWrapper> CommentCreate(CreateCommentDto createComment)
        {
            return await _commentService.CommentCreateAsync(createComment);
        }

        [HttpPut]
        public async Task<ResponseWrapper> CommentUpdate(Guid commentId, CreateCommentDto updateComment)
        {
            return await _commentService.CommentUpdateAsync(commentId, updateComment);
        }

        [HttpGet("{commentId}")]
        public async Task<ResponseWrapper> GetComment(Guid commentId)
        {
            return await _commentService.GetCommentAsync(commentId);
        }

        [HttpGet]
        public async Task<ResponseWrapper> GetAllComment()
        {
            return await _commentService.GetAllCommentAsync();
        }

        [HttpGet("CommentWithReplies/{commentId}")]
        public async Task<ResponseWrapper> GetCommentWithReplies(Guid commentId)
        {
            return await _commentService.GetCommentWithRepliesAsync(commentId);
        }

        [HttpGet("CommentsWithReplies")]
        public async Task<ResponseWrapper> GetCommentsWithReplies()
        {
            return await _commentService.GetCommentsWithRepliesAsync();
        }

        [HttpDelete]
        public async Task<ResponseWrapper> DeleteComment(Guid commentId)
        {
            return await _commentService.DeleteCommentAsync(commentId);
        }

    }
}
