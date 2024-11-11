using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        public string? PersonalUrl { get; set; }
        public string? Nickname { get; set; }
        public bool IsContactInfoPublic { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public string? Organization { get; set; }
        public string? Country { get; set; }
        public string? SocialLinks { get; set; }
    }
}
