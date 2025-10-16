using myapp.Enums;

namespace myapp.auth.Models
{
    public class Resources
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string MediaUrl { get; set; } = "";
        public bool Deleted { get; set; } = false;
        public ResourcesType ResourcesType { get; set; } = ResourcesType.article;
    }
}
