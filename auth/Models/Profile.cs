using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace myapp.auth.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Subscription { get; set; }
        public int TrainingProgress { get; set; }
        public int userScore { get; set; } = 0;
        public List<string>? Achievements { get; set; }
        public List<TrainingHistoryItem>? TrainingHistory { get; set; }
        public List<CertificateItem>? Certificates { get; set; }

        [Required]
        public int UserId { get; set; }
    }


    public class TrainingHistoryItem
    {
        public string? Title { get; set; }
        public string? Status { get; set; }
    }

    public class CertificateItem
    {
        public string? Title { get; set; }
        public string? PreviewUrl { get; set; }
        public string? DownloadUrl { get; set; }
    }
}