using BlogService.Application.DTOs;
using BlogService.Common.Utilities;

namespace BlogService.Core.Interfaces
{
    public interface IUserService
    {
        Task<ResponseWrapper> CreateAsync(CreateUserDto createUser);
        Task<ResponseWrapper> UpdateAsync(UpdateUserDto updateUser, string userId);
        Task<ResponseWrapper> LoginAsync(LoginDto login);
        Task<ResponseWrapper> DeleteAsync(string userId);
        Task<ResponseWrapper> GetUserAsync(string userId);
        Task<ResponseWrapper> GetUsersAsync();
        Task<ResponseWrapper> ChangePasswordAsync(ChangePasswordDto changePassword);
    }
}
