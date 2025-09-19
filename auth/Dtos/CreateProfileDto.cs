using System.ComponentModel.DataAnnotations;

namespace myapp.auth.Dtos
{
    public class CreateProfileDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Subscription { get; set; }
        public int TrainingProgress { get; set; }
        public int userScore { get; set; } = 0;
        public List<string>? Achievements { get; set; }
        public List<TrainingHistoryItemDto>? TrainingHistory { get; set; }
        public List<CertificateItemDto>? Certificates { get; set; }
    }
}

