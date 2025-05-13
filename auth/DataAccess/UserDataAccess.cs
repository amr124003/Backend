using System;
using System.Threading.Tasks;
using myapp.auth.Models;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
namespace myapp.DataAccess
{

    public class UserDataAccess
    {
        private readonly
  ApplicationDbContext _context;

        public UserDataAccess(ApplicationDbContext context
)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user); 
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
            await Task.CompletedTask; // Placeholder
            return null;

        }

        /// Finds a user by their username.
        /// <param name="username">The username to search for.</param>
        /// <returns>The User object if found, otherwise null.</returns>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }



        /// <param name="email">The email address to search for.</param>
        /// <returns>The User object if found, otherwise null.</returns>
        public async Task<User?> FindUserByEmailAsync(string email)
        {
            // Example using Entity Framework Core: return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            // Add database interaction logic here to find the user by email
            await Task.CompletedTask; // Placeholder
            return null;

            // return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


        /// <param name="email">The email address to search for.</param>
        /// <returns>The User object if found, otherwise null.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            // Example using Entity Framework Core: return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            // Add database interaction logic here to find the user by email
            await Task.CompletedTask; // Placeholder
            return null;
        }
    }
}