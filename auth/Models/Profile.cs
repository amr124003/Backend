namespace myapp.auth.Models
{
    public class Profile
    {
        public string Id { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PhotoUrl { get; set; }
        public string Subscription { get; set; } = "Basic";
        public int TrainingProgress { get; set; } // percent
        public List<string> Achievements { get; set; } = new();
        public List<TrainingHistoryItem> TrainingHistory { get; set; } = new();
        public List<CertificateItem> Certificates { get; set; } = new();
    }

    public class TrainingHistoryItem
    {
        public string Title { get; set; } = default!;
        public string Status { get; set; } = default!; // e.g., "Completed", "In Progress"
    }

    public class CertificateItem
    {
        public string Title { get; set; } = default!;
        public string PreviewUrl { get; set; } = default!;
        public string DownloadUrl { get; set; } = default!;
    }
}
