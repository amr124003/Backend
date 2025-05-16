using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapp.auth.Models;
using myapp.Data;

namespace myapp.auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetAllPlans()
        {
            var plans = await _context.Plans
                .Include(p => p.Options)
                    .ThenInclude(o => o.Features)
                .ToListAsync();
            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlanById(int id)
        {
            var plan = await _context.Plans
                .Include(p => p.Options)
                    .ThenInclude(o => o.Features)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null) return NotFound();
            return Ok(plan);
        }

    }
}
