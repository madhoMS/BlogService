namespace BlogService.Application.DTOs
{
    public class ChangePasswordDto
    {
        public string userId { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
