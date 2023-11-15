namespace BlogService.Application.DTOs
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
