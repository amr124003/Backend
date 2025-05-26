namespace myapp.auth.Models
{
    public class LeaderboardEntryDto
    {

        public int Rank { get; set; }
        public string? PhotoUrl { get; set; }
        public string? FullName { get; set; }
        public int Score { get; set; }
    }
}
