using BlogService.Application.DTOs;
using BlogService.Common.Utilities;
using BlogService.Core.Interfaces;
using BlogService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;
using System.Net;
using System.Xml.Linq;

namespace BlogService.Infrastructure.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IReplyRepository _replyRepository;
        private readonly ILogger<ReplyService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public ReplyService(IReplyRepository replyRepository, ILogger<ReplyService> logger, ICacheService cacheService, IConfiguration configuration)
        {
            this._replyRepository = replyRepository;
            this._logger = logger;
            this._cacheService = cacheService;
            this._configuration = configuration;
        }
        public async Task<ResponseWrapper> ReplyCreateAsync(CreateReplyDto createReply)
        {
            try
            {
                Reply reply = new Reply
                {
                    Content = createReply.Content,
                    CreatedAt = DateTime.Now,
                    CommentId = createReply.CommentId,
                    UserId = createReply.UserId,
                    parentReplyId = createReply.ParentReplyId,
                    ImageUrl = Common.Utilities.Common.UploadImage(createReply.Base64Image, "Reply")

                };
                var result = await _replyRepository.AddAsync(reply);

                IEnumerable<Reply> replies = await _replyRepository.GetRepliesAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Reply>>("Reply", replies, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Reply successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = result
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when create reply");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> ReplyUpdateAsync(Guid replyId, CreateReplyDto updateReply)
        {
            try
            {
                Reply reply = await _replyRepository.GetByIdAsync(replyId);
                if (reply == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No reply found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                reply.Content = updateReply.Content;
                reply.ModifiedAt = DateTime.Now;
                if (!string.IsNullOrEmpty(updateReply.Base64Image))
                {
                    reply.ImageUrl = Common.Utilities.Common.UploadImage(updateReply.Base64Image, "Reply");
                }

                await _replyRepository.UpdateAsync(reply);

                IEnumerable<Reply> replies = await _replyRepository.GetRepliesAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Reply>>("Reply", replies, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Reply updated successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = reply
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when update reply");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetReplyAsync(Guid replyId)
        {
            try
            {
                Reply reply = new Reply();
                var cacheData = _cacheService.GetData<IEnumerable<Reply>>("Reply");
                if (cacheData != null)
                {
                    reply = cacheData.Where(x => x.Id == replyId).FirstOrDefault();
                    if (reply != null)
                    {
                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "Reply fetched successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = reply
                        };
                    }
                }

                reply = await _replyRepository.GetReplyAsync(replyId);

                if (reply == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No reply found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Reply fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = reply
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when feteched specific reply");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetAllReplyAsync()
        {
            try
            {
                var cacheData = _cacheService.GetData<IEnumerable<Reply>>("Reply");
                if (cacheData != null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "Replies fetched successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = cacheData
                    };
                }


                IEnumerable<Reply> replies = await _replyRepository.GetRepliesAsync();

                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<Reply>>("Reply", replies, expirationTime);

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Replies fetched successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = replies
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when feteched List of replies");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> DeleteReplyAsync(Guid replyId)
        {
            try
            {
                Reply reply = await _replyRepository.GetByIdAsync(replyId);

                if (reply == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = true,
                        Message = "No reply found!",
                        StatusCode = (int)HttpStatusCode.OK,
                        data = new object()
                    };
                }

                await _replyRepository.DeleteAsync(reply);
                _cacheService.RemoveData("Reply");

                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = "Reply deleted successfully!",
                    StatusCode = (int)HttpStatusCode.OK,
                    data = new object()
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when deleting reply");
                return new ResponseWrapper
                {
                    IsSuccess = true,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }
    }
}
