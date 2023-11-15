using BlogService.Application.DTOs;
using BlogService.Common.Utilities;
using BlogService.Core.Interfaces;
using BlogService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace BlogService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly ILogger<UserService> _logger;
        private readonly ICacheService _cacheService;

        public UserService(IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            RoleManager<Roles> roleManager, ILogger<UserService> logger, ICacheService cacheService)
        {
            this._configuration = configuration;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._logger = logger;
            this._cacheService = cacheService;
        }

        public async Task<ResponseWrapper> CreateAsync(CreateUserDto createUser)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var user = new ApplicationUser
                    {
                        Email = createUser.Email,
                        UserName = createUser.Email,
                        FirstName = createUser.FirstName,
                        LastName = createUser.LastName,
                        ImageUrl = Common.Utilities.Common.UploadImage(createUser.Base64Image, "User"),

                    };

                    ApplicationUser applicationUser = await _userManager.FindByEmailAsync(createUser.Email);
                    if (applicationUser != null)
                    {
                        return new ResponseWrapper
                        {
                            IsSuccess = false,
                            Message = "Email Id already exists!",
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            data = new object()
                        };
                    }

                    var result = await _userManager.CreateAsync(user, createUser.Password);
                    if (result.Succeeded)
                    {
                        var createdData = new UserResponseDto
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            UserId = user.Id
                        };

                        _cacheService.RemoveData("Users");

                        var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                        var cacheData = await _userManager.Users.ToListAsync();                      
                        _cacheService.SetData<IEnumerable<ApplicationUser>>("Users", cacheData, expirationTime);
                        scope.Complete();
                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "User created successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = createdData
                        };
                    }

                    return new ResponseWrapper
                    {
                        IsSuccess = false,
                        Message = result.Errors,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        data = new object()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when create user");
                return new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> UpdateAsync(UpdateUserDto updateUser, string userId)
        {
            try
            {
                DateTime now = DateTime.Now;
                var existsUser = await _userManager.FindByEmailAsync(updateUser.Email);
                if (existsUser != null)
                {
                    if (existsUser.Id != new Guid(userId))
                    {
                        return new ResponseWrapper
                        {
                            IsSuccess = false,
                            Message = "Email Id already exists!",
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            data = new object()
                        };
                    }
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.Email = updateUser.Email;
                    user.UserName = updateUser.Email;
                    user.FirstName = updateUser.FirstName;
                    user.LastName = updateUser.LastName;
                    if (!string.IsNullOrEmpty(updateUser.Base64Image))
                    {
                        user.ImageUrl = Common.Utilities.Common.UploadImage(updateUser.Base64Image, "User");
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _cacheService.RemoveData("Users");

                        var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                        var cacheData = await _userManager.Users.ToListAsync();
                        _cacheService.SetData<IEnumerable<ApplicationUser>>("Users", cacheData, expirationTime);

                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "User updated successfully",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = updateUser
                        };
                    }
                    return new ResponseWrapper
                    {
                        IsSuccess = false,
                        Message = result.Errors,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        data = new object()
                    };
                }

                return new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = "User not found!",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    data = new object()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when update user");
                return new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> DeleteAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (user != null)
                    {
                        user.IsDeleted = true;
                        await _userManager.UpdateAsync(user);
                        _cacheService.RemoveData("Users");
                        scope.Complete();
                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "User deleted successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = new object()
                        };
                    }
                }
                return new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = "User not found!",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    data = new object()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when delete user");
                return new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> GetUserAsync(string userId)
        {
            try
            {
                ApplicationUser filteredData;
                var cacheData = _cacheService.GetData<IEnumerable<ApplicationUser>>("Users");
                if (cacheData != null)
                {
                    filteredData = cacheData.Where(x => x.Id == new Guid(userId)).FirstOrDefault();
                    return new ResponseWrapper()
                    {
                        IsSuccess = true,
                        data = PrepareResponse(filteredData),
                        Message = "User fected successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                
                var result = await _userManager.FindByIdAsync(userId);
                if (result != null)
                {
                    return new ResponseWrapper()
                    {
                        IsSuccess = true,
                        data = PrepareResponse(result),
                        Message = "User fected successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }

                return new ResponseWrapper()
                {
                    IsSuccess = false,
                    data = new object(),
                    Message = "User not found!",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when feteched user");
                return new ResponseWrapper()
                {
                    IsSuccess = false,
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<ResponseWrapper> GetUsersAsync()
        {
            try
            {                
                var cacheData = _cacheService.GetData<IEnumerable<ApplicationUser>>("Users");
                if (cacheData != null)
                {
                    return new ResponseWrapper()
                    {
                        IsSuccess = true,
                        data = PrepareResponse(cacheData.ToList()),
                        Message = "User fected successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }

                var result = await _userManager.Users.ToListAsync();
                var expirationTime = DateTimeOffset.Now.AddMinutes(Convert.ToInt64(_configuration["Radis:ExpireTime"].ToString()));
                _cacheService.SetData<IEnumerable<ApplicationUser>>("Users", result, expirationTime);
                if (result != null)
                {
                    return new ResponseWrapper()
                    {
                        IsSuccess = true,
                        data = PrepareResponse(result),
                        Message = "User fected successfully!",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }

                return new ResponseWrapper()
                {
                    IsSuccess = false,
                    data = new object(),
                    Message = "User not found!",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when feteched user");
                return new ResponseWrapper()
                {
                    IsSuccess = false,
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<ResponseWrapper> LoginAsync(LoginDto login)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(login.Email);
                if (user == null)
                {
                    return new ResponseWrapper
                    {
                        IsSuccess = false,
                        Message = "Email not found!",
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        data = new object()
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Generate and return JWT token
                    var token = await GenerateJwtToken(user.UserName);
                    if (token != null)
                    {
                        var loginResult = new UserResponseDto
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            UserId = user.Id,
                            Image = user.ImageUrl,
                            Token = Convert.ToString(token)
                        };

                        return new ResponseWrapper
                        {
                            IsSuccess = true,
                            Message = "Login successfully!",
                            StatusCode = (int)HttpStatusCode.OK,
                            data = loginResult
                        };
                    }

                    return new ResponseWrapper
                    {
                        IsSuccess = false,
                        Message = "Invalide request!",
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        data = new object()
                    };
                }

                return new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = "Invalide password!",
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    data = new object()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when Login user");
                return new ResponseWrapper
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    data = new object()
                };
            }
        }

        public async Task<ResponseWrapper> ChangePasswordAsync(ChangePasswordDto changePassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(changePassword.userId);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, changePassword.currentPassword, changePassword.newPassword);
                    if (result.Succeeded)
                    {
                        return new ResponseWrapper
                        {
                            data = new object(),
                            IsSuccess = true,
                            Message = "Password updated successfully!",
                            StatusCode = (int)HttpStatusCode.OK
                        };
                    }

                    return new ResponseWrapper
                    {
                        data = new object(),
                        IsSuccess = false,
                        Message = result.Errors,
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }

                return new ResponseWrapper
                {
                    data = new object(),
                    IsSuccess = false,
                    Message = "User not found!",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when user change password");
                return new ResponseWrapper()
                {
                    IsSuccess = false,
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }
        }

        private async Task<string> GenerateJwtToken(string username)
        {
            ApplicationUser applicationUser = await _userManager.FindByEmailAsync(username);
            if (applicationUser != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, applicationUser?.FirstName+" "+ applicationUser?.LastName),
                new Claim(ClaimTypes.Email, applicationUser?.Email),
                new Claim(JwtRegisteredClaimNames.Sid, applicationUser?.Id.ToString()),
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:TokenSecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:TokenExpiryHours"]));

                var token = new JwtSecurityToken(
                    _configuration["JWT:TokenIssuer"],
                    _configuration["JWT:TokenAudience"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return string.Empty;
            }

        }

        private UserResponseDto PrepareResponse(ApplicationUser user)
       => new UserResponseDto()
       {
           Email = user.Email,
           FirstName = user.FirstName,
           LastName = user.LastName,
           UserId = user.Id,
           Image = user.ImageUrl,
           IsDeleted = user.IsDeleted,
       };


        private List<UserResponseDto> PrepareResponse(List<ApplicationUser> users)
       => users.Select(u=> new UserResponseDto()
       {
           Email = u.Email,
           FirstName = u.FirstName,
           LastName = u.LastName,
           UserId = u.Id,
           Image = u.ImageUrl,
           IsDeleted = u.IsDeleted,
       }).ToList();
    }
}
