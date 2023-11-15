using BlogService.Application.DTOs;
using BlogService.Common.Utilities;
using BlogService.Core.Interfaces;
using BlogService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;

namespace BlogService.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public PostService(IPostRepository postRepository, ILogger<PostService> logger, ICacheService cacheService, IConfiguration configuration)
        {
            this._postRepository = postRepository;
            this._logger = logger;
            this._cacheService = cacheService;
            this._configuration = configuration;
        }

        public async Task<ResponseWrapper> PostCreateAsync(CreatePostDto createPost)
        {
            try
            {
                Post post = new Post
                {
                    Title = createPost.Title,
                    Content = createPost.Content,
                    UserId = createPost.UserId,
                    CreatedAt = DateTime.Now,
                    ImageUrl = Common.Utilities.Common.UploadImage(createPost.Base64Image, "Post")
                };
                var result = await _postRepository.AddAsync(post);


                _cacheService.RemoveData("Posts");
                _cacheService.RemoveData("PostsComment");

                IEnumerable<Post> posts = await _postRepository.GetPostsWithCommentsAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Post>>("PostsComment", posts, expirationTime);
                _cacheService.SetData<IEnumerable<Post>>("Posts", posts, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Post successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = result
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when create post");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> PostUpdateAsync(Guid PostId, CreatePostDto updatePost)
        {
            try
            {
                Post post = await _postRepository.GetByIdAsync(PostId);
                if (post == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No post found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                post.Title = updatePost.Title;
                post.Content = updatePost.Content;
                post.UserId = updatePost.UserId;
                post.ModifiedAt = DateTime.Now;
                if (!string.IsNullOrEmpty(updatePost.Base64Image))
                {
                    post.ImageUrl = Common.Utilities.Common.UploadImage(updatePost.Base64Image, "Post");
                }

                await _postRepository.UpdateAsync(post);

                _cacheService.RemoveData("Posts");
                _cacheService.RemoveData("PostsComment");

                IEnumerable<Post> posts = await _postRepository.GetPostsWithCommentsAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Post>>("PostsComment", posts, expirationTime);
                _cacheService.SetData<IEnumerable<Post>>("Posts", posts, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Post updated successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = post
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when uodate post");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetAllPostAsync()
        {
            try
            {
                var cacheData = _cacheService.GetData<IEnumerable<Post>>("Posts");
                if (cacheData != null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "Posts fetched successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = cacheData
                    };
                }

                IEnumerable<Post> posts = await _postRepository.GetAllAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));                
                _cacheService.SetData<IEnumerable<Post>>("Posts", posts, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Posts fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = posts
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when feteched List of posts");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetAllPostWithCommentAndRepliesAsync()
        {
            try
            {
                var cacheData = _cacheService.GetData<IEnumerable<Post>>("PostsComment");
                if (cacheData != null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "Posts fetched successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = cacheData
                    };
                }


                IEnumerable<Post> posts = await _postRepository.GetPostsWithCommentsAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Post>>("PostsComment", posts, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Posts fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = PreparePostsResponse(posts)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when feteched List of posts");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetPostAsync(Guid PostId)
        {
            try
            {
                Post post = new Post();
                var cacheData = _cacheService.GetData<IEnumerable<Post>>("Posts");
                if (cacheData != null)
                {
                    post = cacheData.Where(x => x.Id == PostId).FirstOrDefault();
                    if (post != null)
                    {
                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "Post fetched successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = post
                        };
                    }
                }

                post = await _postRepository.GetByIdAsync(PostId);

                if (post == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No post found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Post fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = post
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when feteched the post");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> DeletePostAsync(Guid PostId)
        {
            try
            {
                Post post = await _postRepository.GetByIdAsync(PostId);

                if (post == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No post found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                await _postRepository.DeleteAsync(post);

                _cacheService.RemoveData("Posts");
                _cacheService.RemoveData("PostsComment");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Post deleted successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = new object()
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when deleting the post");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetPostWithCommentsAsync(Guid PostId)
        {
            try
            {
                Post post = new Post();
                var cacheData = _cacheService.GetData<IEnumerable<Post>>("PostsComment");
                if (cacheData != null)
                {
                    post = cacheData.Where(x => x.Id == PostId).FirstOrDefault();
                    if (post != null)
                    {
                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "Post fetched successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = post
                        };
                    }
                }

                post = await _postRepository.GetPostWithCommentsAsync(PostId);

                if (post == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No post found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Post fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = PreparePostResponse(post)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when fetech the post and comment");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        private PostDto PreparePostResponse(Post post)
       => new PostDto()
       {

           Id = post.Id,
           Title = post.Title,
           Content = post.Content,
           ImageUrl = post.ImageUrl,
           UserName = post.User.FirstName + " " + post.User.LastName,
           UserId = post.UserId,
           Comments = PrepareCommentResponse(post.Comments),
       };

        private IEnumerable<PostDto> PreparePostsResponse(IEnumerable<Post> posts)
       => posts.Select(p => new PostDto()
       {

           Id = p.Id,
           Title = p.Title,
           Content = p.Content,
           ImageUrl = p.ImageUrl,
           UserName = p.User.FirstName + " " + p.User.LastName,
           UserId = p.UserId,
           Comments = PrepareCommentResponse(p.Comments),
       }).ToList();

        private List<CommentDto> PrepareCommentResponse(IList<Comment> comments)
        => comments.Select(s => new CommentDto()
        {
            Id = s.Id,
            Content = s.Content,
            ImageUrl = s.ImageUrl,
            UserName = s.User.FirstName + " " + s.User.LastName,
            PostId = s.PostId,
            UserId = new Guid(s.UserId.ToString()),
            Replies = PrepareReplyResponse(s.Replies)
        }).ToList();

        private List<ReplyDto> PrepareReplyResponse(IList<Reply> replies)
        => replies.Select(s => new ReplyDto()
        {
            Id = s.Id,
            Content = s.Content,
            ImageUrl = s.ImageUrl,
            UserName = s.User.FirstName + " " + s.User.LastName,
            CommentId = s.CommentId,
            UserId = new Guid(s.UserId.ToString()),
        }).ToList();
    }
}
