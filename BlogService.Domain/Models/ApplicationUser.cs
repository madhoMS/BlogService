using Microsoft.AspNetCore.Identity;

namespace BlogService.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}
