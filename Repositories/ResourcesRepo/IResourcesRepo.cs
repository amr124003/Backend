using myapp.auth.Dtos;
using myapp.auth.Models;
using myapp.Enums;
using System.Runtime.InteropServices;

namespace myapp.Repositories.ResourcesRepo
{
    public interface IResourcesRepo
    {
        public Task<Resources?> CreateResources ( CreateResoucesDto model );
        public Task<bool> UpdateResources ( UpdateResourcesDto model );
        public Task<bool> DeleteResources ( int Id );
        public Task<object> GetAllResources (ResourcesType ResourcesType);
        public Task<Resources?> GetResource (int Id , ResourcesType ResourcesType );
    }
}
