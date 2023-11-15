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
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            this._postService = postService;
        }

        [HttpPost]
        public async Task<ResponseWrapper> PostCreate(CreatePostDto createPost)
        {
            return await _postService.PostCreateAsync(createPost);
        }

        [HttpPut]
        public async Task<ResponseWrapper> PostUpdate(Guid postId, CreatePostDto updatePost)
        {
            return await _postService.PostUpdateAsync(postId, updatePost);
        }

        [HttpGet("{postId}")]
        public async Task<ResponseWrapper> GetPost(Guid postId)
        {
            return await _postService.GetPostAsync(postId);
        }

        [HttpGet("PostWithCommentsandReplies/{postId}")]
        public async Task<ResponseWrapper> PostWithComment(Guid postId)
        {
            return await _postService.GetPostWithCommentsAsync(postId);
        }

        [HttpGet("PostsWithCommentsandReplies")]
        public async Task<ResponseWrapper> PostsWithCommentsandReplies()
        {
            return await _postService.GetAllPostWithCommentAndRepliesAsync();
        }

        [HttpGet]
        public async Task<ResponseWrapper> GetAllPost()
        {
            return await _postService.GetAllPostAsync();
        }

        [HttpDelete]
        public async Task<ResponseWrapper> DeletePost(Guid postId)
        {
            return await _postService.DeletePostAsync(postId);
        }
    }
}
