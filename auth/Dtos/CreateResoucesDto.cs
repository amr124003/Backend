using myapp.Enums;

namespace myapp.auth.Dtos
{
    public class CreateResoucesDto
    {
        public string Title { get; set; } 
        public string Description { get; set; }
        public IFormFile Media { get; set; }
        public bool Deleted { get; set; } 
        public ResourcesType ResourcesType { get; set; } 
    }
}
