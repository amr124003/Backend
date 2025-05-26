using System;
using System.Threading.Tasks;
using myapp.auth.Models;
using Microsoft.EntityFrameworkCore;
using myapp.Data;

namespace myapp.DataAccess
{
    public class UserDataAccess
    {
        private readonly ApplicationDbContext _context;

        public UserDataAccess(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create and link a profile for the new user
                var profile = new Profile
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    TrainingProgress = 0,
                    userScore = 0,
                    Achievements = new List<string>(),
                    TrainingHistory = new List<TrainingHistoryItem>(),
                    Certificates = new List<CertificateItem>()
                };
                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();

                // Optionally set ProfileId in User if you want to keep this reference
                user.Profile.Id = profile.Id;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
