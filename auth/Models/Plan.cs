using System.Text.Json.Serialization;

namespace myapp.auth.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<PlanOption>? Options { get; set; }
        public string? Icon { get; set; }
    }

    public class PlanOption
    {
        public int Id { get; set; }
        public int DurationMonths { get; set; }
        public string? Label { get; set; }
        public decimal Price { get; set; }
        public string? PriceUnit { get; set; }
        public string? Note { get; set; }
        public List<PlanFeature>? Features { get; set; }
        public int PlanId { get; set; }
        [JsonIgnore]
        public Plan? Plan { get; set; }
    }

    public class PlanFeature
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int PlanOptionId { get; set; }
        [JsonIgnore]
        public PlanOption? PlanOption { get; set; }
    }
}
