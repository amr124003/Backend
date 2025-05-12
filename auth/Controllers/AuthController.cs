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

public class AuthController : ControllerBase
{
    private readonly UserDataAccess _userDataAccess;
    private readonly IConfiguration _configuration;

    public AuthController(UserDataAccess userDataAccess, IConfiguration configuration)
    {
        _userDataAccess = userDataAccess;
        _configuration = configuration;
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

        Console.WriteLine($"Stored Password: {user.Password}");
        Console.WriteLine($"Provided Password: {model.Password}");

        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            Console.WriteLine("Password verification failed.");
            return Unauthorized("Invalid username or password.");
        }

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        });
    }
}

