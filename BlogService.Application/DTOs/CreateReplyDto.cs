namespace BlogService.Application.DTOs
{
    public class CreateReplyDto
    {
        public Guid CommentId { get; set; }
        public Guid? ParentReplyId { get; set; }
        public string Content { get; set; }
        public string Base64Image { get; set; }
        public Guid UserId { get; set; }
    }
}
