using myapp.Enums;

namespace myapp.auth.Dtos
{
    public class UpdateResourcesDto
    {
        public int Id { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; } 
        public IFormFile? Media { get; set; } 
    }
}
