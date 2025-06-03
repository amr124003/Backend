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
        public required string mail { get; set; }
        public required string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public required string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public required string Otp { get; set; }
        public required string NewPassword { get; set; }
    }

}
