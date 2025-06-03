using System.Text.Json.Serialization;

namespace myapp.auth.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Username { get; set; }
        public Profile? Profile { get; set; }

        // these for password reset
        public string? PasswordResetOtp { get; set; }
        public DateTime? PasswordResetOtpExpiry { get; set; }
    }
}