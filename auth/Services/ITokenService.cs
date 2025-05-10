using myapp.auth.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace myapp.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}