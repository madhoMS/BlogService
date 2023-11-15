namespace BlogService.Domain.Models
{
    public class Reply
    {
        public Guid Id { get; set; }
        public Guid? parentReplyId { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }

    }
}
