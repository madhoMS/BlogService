using BlogService.Application.DTOs;
using BlogService.Common.Utilities;
using BlogService.Core.Interfaces;
using BlogService.Domain.Models;
using BlogService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BlogService.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<CommentService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public CommentService(ICommentRepository commentRepository, ILogger<CommentService> logger, ICacheService cacheService, IConfiguration configuration)
        {
            this._commentRepository = commentRepository;
            this._logger = logger;
            this._cacheService = cacheService;
            this._configuration = configuration;
        }

        public async Task<ResponseWrapper> CommentCreateAsync(CreateCommentDto createComment)
        {
            try
            {
                Comment comment = new Comment
                {
                    Content = createComment.Content,
                    CreatedAt = DateTime.Now,
                    PostId = createComment.PostId,
                    UserId = createComment.UserId,
                    ImageUrl = Common.Utilities.Common.UploadImage(createComment.Base64Image, "Comment")

                };
                var result = await _commentRepository.AddAsync(comment);

                _cacheService.RemoveData("Comments");
                _cacheService.RemoveData("CommentsReplies");

                IEnumerable<Comment> commentsReplies = await _commentRepository.GetCommentsWithRepliesAsync();
                IEnumerable<Comment> comments = await _commentRepository.GetAllAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Comment>>("CommentsReplies", commentsReplies, expirationTime);
                _cacheService.SetData<IEnumerable<Comment>>("Comments", comments, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Comment successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = result
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when create comment");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> CommentUpdateAsync(Guid commentId, CreateCommentDto updateComment)
        {
            try
            {
                Comment comment = await _commentRepository.GetByIdAsync(commentId);
                if (comment == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No comment found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                comment.Content = updateComment.Content;
                comment.ModifiedAt = DateTime.Now;
                if (!string.IsNullOrEmpty(updateComment.Base64Image))
                {
                    comment.ImageUrl = Common.Utilities.Common.UploadImage(updateComment.Base64Image, "Comment");
                }

                await _commentRepository.UpdateAsync(comment);

                _cacheService.RemoveData("Comments");
                _cacheService.RemoveData("CommentsReplies");

                IEnumerable<Comment> commentsReplies = await _commentRepository.GetCommentsWithRepliesAsync();
                IEnumerable<Comment> comments = await _commentRepository.GetAllAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Comment>>("CommentsReplies", commentsReplies, expirationTime);
                _cacheService.SetData<IEnumerable<Comment>>("Comments", comments, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Comment updated successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = comment
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when update comment");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }
        
        public async Task<ResponseWrapper> GetAllCommentAsync()
        {
            try
            {
                var cacheData = _cacheService.GetData<IEnumerable<Comment>>("Comments");
                if (cacheData != null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "Comments fetched successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = cacheData
                    };
                }

               IEnumerable<Comment> comments = await _commentRepository.GetAllAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Comment>>("Comments", comments, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Comments fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = comments
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when fetched List of comments");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }
       
        public async Task<ResponseWrapper> GetCommentAsync(Guid commentId)
        {
            try
            {
                Comment comment = new Comment();
                var cacheData = _cacheService.GetData<IEnumerable<Comment>>("Comments");
                if (cacheData != null)
                {
                    comment = cacheData.Where(x => x.Id == commentId).FirstOrDefault();
                    if (comment != null)
                    {
                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "Comment fetched successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = comment
                        };
                    }
                }

                comment = await _commentRepository.GetByIdAsync(commentId);

                if (comment == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No comment found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Comment fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = comment
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when fetched specified comment");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }
       
        public async Task<ResponseWrapper> DeleteCommentAsync(Guid commentId)
        {
            try
            {
                Comment comment = await _commentRepository.GetByIdAsync(commentId);

                if (comment == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No comment found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                await _commentRepository.DeleteAsync(comment);

                _cacheService.RemoveData("Comments");
                _cacheService.RemoveData("CommentsReplies");

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Comment deleted successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = new object()
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when deleting comment");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetCommentWithRepliesAsync(Guid commentId)
        {
            try
            {
                Comment comment = new Comment();
                var cacheData = _cacheService.GetData<IEnumerable<Comment>>("CommentsReplies");
                if (cacheData != null)
                {
                    comment = cacheData.Where(x => x.Id == commentId).FirstOrDefault();
                    if (comment != null)
                    {
                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "Comment fetched successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = PrepareCommentResponse(comment)
                        };
                    }
                }
                comment = await _commentRepository.GetCommentWithRepliesAsync(commentId);

                if (comment == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No comment found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Comment fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = PrepareCommentResponse(comment)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when fetech the comment and replies");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetCommentsWithRepliesAsync()
        {
            try
            {
                var cacheData = _cacheService.GetData<IEnumerable<Comment>>("CommentsReplies");
                if (cacheData != null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "Comments fetched successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = PrepareCommentsResponse(cacheData)
                    };
                }


                IEnumerable<Comment> comments = await _commentRepository.GetCommentsWithRepliesAsync();

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Comments fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = PrepareCommentsResponse(comments)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when fetched List of comments");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        private CommentDto PrepareCommentResponse(Comment comment)
       => new CommentDto()
       {

           Id = comment.Id,
           Content = comment.Content,
           ImageUrl = comment.ImageUrl,
           UserName = comment.User.FirstName + " " + comment.User.LastName,
           UserId = new Guid(comment.UserId.ToString()),
           Replies = PrepareReplyResponse(comment.Replies),
       };

        private IEnumerable<CommentDto> PrepareCommentsResponse(IEnumerable<Comment> comments)
       => comments.Select(c => new CommentDto()
       {

           Id = c.Id,
           Content = c.Content,
           ImageUrl = c.ImageUrl,
           UserName = c.User.FirstName + " " + c.User.LastName,
           UserId = new Guid(c.UserId.ToString()),
           Replies = PrepareReplyResponse(c.Replies),
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
