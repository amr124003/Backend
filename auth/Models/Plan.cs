namespace myapp.auth.Models
{
    public class Plan
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<PlanOption> Options { get; set; } = new();
        public List<string> Features { get; set; } = new();
        public string Icon { get; set; } = default!; // e.g., "1", "2", "crown"
    }

    public class PlanOption
    {
        public int DurationMonths { get; set; }
        public string Label { get; set; } = default!;
        public decimal Price { get; set; }
        public string PriceUnit { get; set; } = "EGP";
        public string Note { get; set; } = default!;
    }
}
