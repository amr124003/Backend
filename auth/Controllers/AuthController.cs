using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using myapp.auth.Models;
using myapp.DataAccess;
using BCrypt.Net;
using myapp.auth.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserDataAccess _userDataAccess;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public AuthController(UserDataAccess userDataAccess, IConfiguration configuration, EmailService emailService)
    {
        _userDataAccess = userDataAccess;
        _configuration = configuration;
        _emailService = emailService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (model.Password != model.ConfirmPassword)
        {
            return BadRequest("Passwords do not match.");
        }

        var existingUser = await _userDataAccess.GetUserByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return Conflict("User with this email already exists.");
        }

        // Hash the password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var newUser = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            Username = model.Email.Split('@')[0],
            Password = hashedPassword,
        };
        await _userDataAccess.CreateUserAsync(newUser);

        return Ok("Signup successful.");
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody] SigninModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userDataAccess.GetUserByEmailAsync(model.mail);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            if (string.IsNullOrEmpty(user.Password) || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Check JWT configuration values
            var keyString = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            if (string.IsNullOrEmpty(keyString) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                return StatusCode(500, "JWT configuration is missing.");
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(keyString);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier , user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMonths(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = token.ValidTo
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "An error occurred during signin.");
        }
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userDataAccess.GetUserByEmailAsync(request.Email);
        if (user == null)
            return NotFound("User not found.");

        // Generate token and expiry
        var otp = new Random().Next(100000, 999999).ToString();
        user.PasswordResetOtp = otp;
        user.PasswordResetOtpExpiry = DateTime.UtcNow.AddMinutes(20);
        await _userDataAccess.UpdateUserAsync(user);

        // Example with EmailService:
        await _emailService.SendEmailAsync(user.Email, "Password Reset OTP", $"Your OTP is: {otp}");

        // For now, return the token for testing
        return Ok("OTP has been sent to your email.");
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var user = await _userDataAccess.GetUserByEmailAsync(request.Email);
        if (user == null || user.PasswordResetOtp != request.Otp || user.PasswordResetOtpExpiry < DateTime.UtcNow)
            return BadRequest("Invalid or expired OTP.");

        // Optionally, mark OTP as used (clear it)
        await _userDataAccess.UpdateUserAsync(user);

        // You may want to issue a short-lived token here for password reset, or just allow the next step
        return Ok("OTP verified. You can now reset your password.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userDataAccess.GetUserByResetOtpAsync(request.Otp);
        if (user == null || user.PasswordResetOtpExpiry < DateTime.UtcNow)
            return BadRequest("Invalid or expired OTP.");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordResetOtp = null;
        user.PasswordResetOtpExpiry = null;
        await _userDataAccess.UpdateUserAsync(user);

        return Ok("Password has been reset.");
    }

}
