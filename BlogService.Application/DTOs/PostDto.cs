namespace BlogService.Application.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
