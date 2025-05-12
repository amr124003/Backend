namespace myapp.auth.Models
{
    public class SignupModel
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public class SigninModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
