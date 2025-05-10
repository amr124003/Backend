using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using myapp.auth.Models; // Assuming your User model is in this namespace
using myapp.DataAccess; // Ensure this using directive is at the very top

public class AuthController : ControllerBase
{
    public class SignupModel    {
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

    private readonly UserDataAccess _userDataAccess;

    public AuthController(UserDataAccess userDataAccess)
    {
        _userDataAccess = userDataAccess;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 1. Check if passwords match
        if (model.Password != model.ConfirmPassword)
        {
            return BadRequest("Passwords do not match.");
        }

        // 2. Check if user already exists by email or username
        var existingUser = await _userDataAccess.GetUserByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return Conflict("User with this email already exists.");
        }
        // You might also want to check for existing username if it's a separate field
        // }

        // TODO: 3. Hash the password
        // Use a library like BCrypt.Net or similar for secure password hashing
        // var hashedPassword = _passwordHasher.HashPassword(model.Password);
        string hashedPassword = "hashed_" + model.Password; // Placeholder

        // TODO: 4. Create a new user in the database
        var newUser = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            Username = model.Email.Split('@')[0], // Explicit placeholder: You need to replace this with your actual username assignment logic
            Password = hashedPassword,
        };
        await _userDataAccess.CreateUserAsync(newUser);

        // 5. Return appropriate response
        return Ok("Signup successful.");
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody] SigninModel model)
    {
 if (!ModelState.IsValid)
        {
 return BadRequest(ModelState);
        }

        // TODO: 1. Find the user by username
 var user = await _userDataAccess.GetUserByUsernameAsync(model.Username);
        if (user == null)
        {
 return Unauthorized("Invalid username or password.");
        }

        // 3. Verify the password (compare hashed password)
        // Use your password hashing library to compare the provided password with the hashed password from the database.
        var isPasswordValid = true; // Placeholder for password verification logic
        if (!isPasswordValid)
        {
            return Unauthorized("Invalid username or password.");
        }

        // 4. Generate a JWT or other authentication token
        // 5. Return the token and user information

        return Ok("Signin successful (placeholder)");
    }
}