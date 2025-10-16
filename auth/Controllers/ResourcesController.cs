using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myapp.auth.Dtos;
using myapp.Enums;
using myapp.Repositories.ResourcesRepo;
using Stripe.Events;
using System.Threading.Tasks;

namespace myapp.auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController(IResourcesRepo resourcesRepo) : ControllerBase
    {
        [HttpPost("CreateResource")]
        public async Task<IActionResult> CreateResources ([FromForm]CreateResoucesDto model)
        {
            var res = await resourcesRepo.CreateResources(model);

            return res != null ? Ok(res) : BadRequest("Error Ocuured In Create");
        }

        [HttpPut("UpdateResource")]
        public async Task<IActionResult> UpdateResource ([FromForm]UpdateResourcesDto model)
        {
            var res = await resourcesRepo.UpdateResources(model);

            return res ? Ok("Update Successfully") : BadRequest("Error Occured In Update");
        }

        [HttpDelete("DeleteResources")]
        public async Task<IActionResult> DeleteResource (int Id)
        {
            var res = await resourcesRepo.DeleteResources(Id);

            return res ? Ok("Deleted Successfully") : BadRequest("Error Occured In Delete");
        }

        [HttpGet("Get Resource")]
        public async Task<IActionResult> GetResourceAsync (ResourcesType Type , int Id)
        {
            var res = await resourcesRepo.GetResource(Id, Type);

            return res != null ? Ok(res) : NotFound("Resource Not Found");
        }

        [HttpGet("GetAllResources")]
        public async Task<IActionResult> GetAllResources(ResourcesType Type)
        {
            var res = await resourcesRepo.GetAllResources(Type);

            return Ok(res);
        }

    }
}
