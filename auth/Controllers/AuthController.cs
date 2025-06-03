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

            var user = await _userDataAccess.GetUserByUsernameAsync(model.Username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return Unauthorized("Invalid username or password.");
            }

            if (string.IsNullOrEmpty(user.Password) || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                Console.WriteLine("Password verification failed.");
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
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
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
        user.PasswordResetToken = Guid.NewGuid().ToString();
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
        await _userDataAccess.UpdateUserAsync(user);

        // Send token to user's email (implement EmailService or return token for testing)
        // Example with EmailService:
        // var resetLink = $"https://your-frontend-url/reset-password?token={user.PasswordResetToken}";
        // await _emailService.SendEmailAsync(user.Email, "Password Reset", $"Reset link: {resetLink}");

        // For now, return the token for testing
        return Ok(new { Token = user.PasswordResetToken });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userDataAccess.GetUserByResetTokenAsync(request.Token);
        if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            return BadRequest("Invalid or expired token.");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        await _userDataAccess.UpdateUserAsync(user);

        return Ok("Password has been reset.");
    }



}
