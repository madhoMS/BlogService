namespace BlogService.Application.DTOs
{
    public class CreateCommentDto
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string Base64Image { get; set; }
        public Guid UserId { get; set; }
    }
}
