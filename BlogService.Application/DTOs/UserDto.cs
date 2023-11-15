namespace BlogService.Application.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
