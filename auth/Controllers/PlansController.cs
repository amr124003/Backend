using Microsoft.AspNetCore.Mvc;
using myapp.auth.Models;

namespace myapp.auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private static readonly List<Plan> Plans = new()
        {
            new Plan
            {
                Id = 1,
                Name = "Get Basic",
                Description = "Remove Add & Unlock All Location",
                Icon = "1",
                Options = new List<PlanOption>
                {
                    new() { DurationMonths = 1, Label = "1 Month", Price = 0, Note = "that's Basic Plan" },
                    new() { DurationMonths = 6, Label = "6 Month", Price = 310, Note = "that's Premium Plan" },
                    new() { DurationMonths = 12, Label = "1 Year", Price = 620, Note = "that's Premium Plan" }
                },
                Features = new List<string>
                {
                    "Limited VR Training",
                    "Access to Home Scenario"
                }
            },
            new Plan
            {
                Id = 2,
                Name = "Get Premium",
                Description = "Remove Add & Unlock All Location",
                Icon = "crown",
                Options = new List<PlanOption>
                {
                    new() { DurationMonths = 1, Label = "1 Month", Price = 0, Note = "that's Basic Plan" },
                    new() { DurationMonths = 6, Label = "6 Month", Price = 310, Note = "that's Premium Plan" },
                    new() { DurationMonths = 12, Label = "1 Year", Price = 620, Note = "that's Premium Plan" }
                },
                Features = new List<string>
                {
                    "AI Chatbot Support",
                    "Home & Factory Scenarios",
                    "Full VR Training Access",
                    "Certification"
                }
            },
            new Plan
            {
                Id = 3,
                Name = "Get Basic",
                Description = "Remove Add & Unlock All Location",
                Icon = "2",
                Options = new List<PlanOption>
                {
                    new() { DurationMonths = 1, Label = "1 Month", Price = 0, Note = "that's Basic Plan" },
                    new() { DurationMonths = 6, Label = "6 Month", Price = 310, Note = "that's Premium Plan" },
                    new() { DurationMonths = 12, Label = "1 Year", Price = 620, Note = "that's Premium Plan" }
                },
                Features = new List<string>
                {
                    "Unlimited VR Training",
                    "All Scenarios (Home, Factory, Vehicle)",
                    "AI Chatbot + Burn Detection",
                    "Multi-User & Custom Reports"
                }
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Plan>> GetAllPlans() => Plans;

        [HttpGet("{id}")]
        public ActionResult<Plan> GetPlanById(int id) // Changed parameter type to int
        {
            var plan = Plans.FirstOrDefault(p => p.Id == id); // No change needed here
            if (plan == null) return NotFound();
            return plan;
        }
    }
}
