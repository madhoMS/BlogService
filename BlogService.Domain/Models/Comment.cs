namespace BlogService.Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public virtual List<Reply> Replies { get; set; }
    }
}
