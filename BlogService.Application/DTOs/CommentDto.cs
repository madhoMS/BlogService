namespace BlogService.Application.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public List<ReplyDto> Replies { get; set; }
    }
}
