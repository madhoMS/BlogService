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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<ResponseWrapper> CreateUser(CreateUserDto createUser)
        {
            var result = await _userService.CreateAsync(createUser);
            return result;
        }

        [HttpPut]
        public async Task<ResponseWrapper> UpdateUser(UpdateUserDto updateUser, string userId)
        {
            var result = await _userService.UpdateAsync(updateUser, userId);
            return result;
        }

        [HttpGet("{userId}")]
        public async Task<ResponseWrapper> GetUserById(string userId)
        {
            var result = await _userService.GetUserAsync(userId);
            return result;
        }

        [HttpGet]
        public async Task<ResponseWrapper> GetUsers()
        {
            var result = await _userService.GetUsersAsync();
            return result;
        }

        [HttpDelete]
        public async Task<ResponseWrapper> DeleteUser(string userId)
        {
            var result = await _userService.DeleteAsync(userId);
            return result;

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ResponseWrapper> Login(LoginDto model)
        {
            var result = await _userService.LoginAsync(model);
            return result;

        }

        [HttpPost("ChangePassword")]
        public async Task<ResponseWrapper> UpdatePassword(ChangePasswordDto changePassword)
        {
            var result = await _userService.ChangePasswordAsync(changePassword);
            return result;

        }
    }
}
