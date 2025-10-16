using Mapster;
using Microsoft.EntityFrameworkCore;
using myapp.auth.Dtos;
using myapp.auth.Models;
using myapp.Data;
using myapp.Enums;

namespace myapp.Repositories.ResourcesRepo
{
    public class ResourcesRepo(ApplicationDbContext context , IHttpContextAccessor request) : IResourcesRepo
    {
        public async Task<Resources?> CreateResources ( CreateResoucesDto model )
        {
            var Resource = model.Adapt<Resources>();
            Resource.MediaUrl = await UploadMedia(model.Media);
            context.Resources.Add(Resource);
            var res = await context.SaveChangesAsync();
            return res > 0 ? Resource : null;
        }

        public async Task<bool> DeleteResources ( int Id )
        {
            var resources = await context.Resources.FirstOrDefaultAsync(x => x.Id == Id);

            if (resources == null)
                return false;

            resources.Deleted = true;
            context.Entry(resources).Property(x => x.Deleted).IsModified = true;
            var res = await context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<object> GetAllResources ( ResourcesType ResourcesType )
        {
            var allResources = await context.Resources.Where(x => !x.Deleted).ToListAsync();

            var Status = allResources.Any();

            return new
            {
                result = new
                {
                    Resources = allResources
                },
                Status = Status
            };
        }

        public async Task<Resources?> GetResource ( int Id, ResourcesType ResourcesType )
        {
            var resources = await context.Resources.FirstOrDefaultAsync(x => x.Id == Id && !x.Deleted);

            if (resources == null)
                return null;

            return resources;
        }

        public async Task<bool> UpdateResources ( UpdateResourcesDto model )
        {
            var resources = await context.Resources.FirstOrDefaultAsync(x => x.Id == model.Id && !x.Deleted);

            if (resources == null)
                return false;

            model.Adapt(resources);

            if(model.Media != null)
                resources.MediaUrl = await UploadMedia(model.Media);

            context.Update(resources);
            var res = await context.SaveChangesAsync();

            return res > 0;
        }

        private async Task<string> UploadMedia(IFormFile file)
        {
            var uploadsFolder = Path.Combine("wwwroot", "Resources");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
                
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var Request = request.HttpContext!.Request;

            return $"{Request.Scheme}://{Request.Host}/Resources/{fileName}";
        }
    }
}
