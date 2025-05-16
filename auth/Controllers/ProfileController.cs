using Microsoft.AspNetCore.Mvc;
using myapp.auth.Models;

namespace myapp.auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private static readonly Dictionary<string, Profile> Profiles = new()
        {
            ["user1"] = new Profile
            {
                Id = 1,
                FullName = "Amera Mahmoud",
                Email = "amera@email.com",
                PhotoUrl = "https://example.com/profile-photo.png",
                Subscription = "Premium",
                TrainingProgress = 60,
                Achievements = new List<string>
                {
                    "Fire Safety Expert",
                    "Car Emergency Responder"
                },
                TrainingHistory = new List<TrainingHistoryItem>
                {
                    new() { Title = "Home Fire Drill", Status = "Completed" },
                    new() { Title = "Factory Safety", Status = "In Progress" }
                },
                Certificates = new List<CertificateItem>
                {
                    new() { Title = "Fire Safety Training", PreviewUrl = "https://example.com/cert1-preview.pdf", DownloadUrl = "https://example.com/cert1.pdf" },
                    new() { Title = "Industrial Safety", PreviewUrl = "https://example.com/cert2-preview.pdf", DownloadUrl = "https://example.com/cert2.pdf" }
                }
            }
        };

        [HttpGet("{id}")]
        public ActionResult<Profile> GetProfile(string id)
        {
            if (!Profiles.TryGetValue(id, out var profile))
                return NotFound("Profile not found.");
            return Ok(profile);
        }

        [HttpPut("{id}")]
        public ActionResult<Profile> UpdateProfile(string id, [FromBody] Profile updated)
        {
            if (!Profiles.ContainsKey(id))
                return NotFound("Profile not found.");

            var profile = Profiles[id];
            profile.FullName = updated.FullName;
            profile.Email = updated.Email;
            profile.PhotoUrl = updated.PhotoUrl;
            // Optionally update other fields as needed
            return Ok(profile);
        }

        [HttpPost]
        public ActionResult<Profile> CreateProfile([FromBody] Profile newProfile)
        {
            var profileKey = newProfile.Id.ToString(); // Convert the integer Id to a string key
            if (Profiles.ContainsKey(profileKey))
                return Conflict("Profile already exists.");

            Profiles[profileKey] = newProfile; // Use the string key for the dictionary
            return CreatedAtAction(nameof(GetProfile), new { id = profileKey }, newProfile);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProfile(string id)
        {
            if (!Profiles.Remove(id))
                return NotFound("Profile not found.");
            return Ok("Profile deleted.");
        }
    }
}
