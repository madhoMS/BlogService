namespace BlogService.Application.DTOs
{
    public class ReplyDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
