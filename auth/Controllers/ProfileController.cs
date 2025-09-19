using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using myapp.auth.Models;
using myapp.Data;
using myapp.auth.Hubs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using myapp.auth.Dtos;
using Mapster;

namespace myapp.auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<LeaderboardHub> _hubContext;

        public ProfileController(ApplicationDbContext context, IHubContext<LeaderboardHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(int id)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null)
                return NotFound("Profile not found.");
            return Ok(profile);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Profile>> UpdateProfile(int id, [FromBody] CreateProfileDto updated)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
                return NotFound("Profile not found.");

            updated.Adapt(profile);
            await _context.SaveChangesAsync();

            // Fetch top 10 leaderboard
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

            var leaderboard = topProfiles
                .Select((p, i) => new LeaderboardEntryDto
                {
                    Rank = i + 1,
                    PhotoUrl = p.PhotoUrl,
                    FullName = p.FullName,
                    Score = p.userScore
                })
                .ToList();

            // Broadcast to all clients
            await _hubContext.Clients.All.SendAsync("LeaderboardUpdated", leaderboard);

            return Ok(profile);
        }

        [HttpPost("CreateProfile")]
        [Authorize]
        public async Task<IActionResult> CreateProfile (CreateProfileDto newProfileDto)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return Unauthorized("Invalid Token");

            if (await _context.Profiles.AnyAsync(p => p.UserId == userId))
                return BadRequest("A profile already exists for this user");

            var newProfile = newProfileDto.Adapt<Profile>();
            newProfile.UserId = userId;

            _context.Profiles.Add(newProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfile), new { id = newProfile.Id }, newProfile);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
                return NotFound("Profile not found.");

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return Ok("Profile deleted.");
        }

        [HttpPost("{id}/photo")]
        public async Task<IActionResult> UploadPhoto(int id, IFormFile file)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
                return NotFound("Profile not found.");

            // Example: Save the file and set the PhotoUrl
            // In production, use a file service and proper validation
            var uploadsFolder = Path.Combine("wwwroot", "profile-photos");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            profile.PhotoUrl = $"/profile-photos/{fileName}";
            await _context.SaveChangesAsync();
            return Ok(profile);
        }

        [HttpDelete("{id}/photo")]
        public async Task<IActionResult> RemovePhoto(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
                return NotFound("Profile not found.");

            profile.PhotoUrl = null;
            await _context.SaveChangesAsync();
            return Ok(profile);
        }
    }
}
