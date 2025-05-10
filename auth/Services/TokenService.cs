using myapp.auth.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace myapp.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _secretKey;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["Jwt:SigningKey"]; // TODO: Ensure SigningKey is configured securely (e.g., via environment variables, Key Vault).

            if (string.IsNullOrEmpty(_secretKey))
            {
                throw new ArgumentNullException("Jwt:SigningKey is not configured.");
            }
        }

        public string GenerateToken(User user)
        {
            // TODO: Implement actual JWT generation logic
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddDays(7), // TODO: Set appropriate expiration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey!)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); // Added null-forgiving operator as _secretKey is checked in the constructor
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            // TODO: Implement actual JWT validation logic
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            try // Removed the null check for _secretKey here as it's already checked in the constructor
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:SigningKey"])),
                    ValidateIssuer = true, // TODO: Validate issuer if applicable
                    ValidIssuer = _configuration["Jwt:Issuer"], // Get issuer from configuration
                    ValidateAudience = true, // TODO: Validate audience if applicable
                    ValidAudience = _configuration["Jwt:Audience"], // Get audience from configuration
                    ClockSkew = TimeSpan.Zero // Optional: to enforce no delay before token expiration
                };

                SecurityToken validatedToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return principal; // Return the ClaimsPrincipal on success
            }
            catch (Exception)
            {
                // Token is invalid
                return null;
            }
        }
    }

}