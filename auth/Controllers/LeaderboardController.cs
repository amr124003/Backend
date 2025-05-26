using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.auth.Models;

namespace myapp.auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Leaderboard/top
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<LeaderboardEntryDto>>> GetTopLeaderboard()
        {
            // Get top 10 profiles ordered by userScore descending
            var topProfiles = await _context.Profiles
                .OrderByDescending(p => p.userScore)
                .ThenBy(p => p.FullName)
                .Take(10)
                .Select(p => new
                {
                    p.PhotoUrl,
                    p.FullName,
                    p.userScore
                })
                .ToListAsync();

            // Assign ranks dynamically
            var leaderboard = topProfiles
                .Select((p, i) => new LeaderboardEntryDto
                {
                    Rank = i + 1,
                    PhotoUrl = p.PhotoUrl,
                    FullName = p.FullName,
                    Score = p.userScore
                })
                .ToList();

            return Ok(leaderboard);
        }
    }
}
