using Application_API.Models;
using Application_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application_API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ShelterController : Controller
    {
        private readonly IShelterService _shelterService;

        public ShelterController(IShelterService shelterService)
        {
            _shelterService = shelterService;
        }

        [HttpGet("all-shelters")]
        public async Task<IActionResult> GetAllSheltersAsync()
        {
            var shelters = await _shelterService.GetAllSheltersAsync();
            return Ok(shelters);
        }

        [HttpGet("shelter-by-id")]
        public async Task<IActionResult> GetShelterByIdAsync(Guid id)
        {
            var shelter = await _shelterService.GetShelterByIdAsync(id);
            if (shelter == null)
            {
                return StatusCode(404, "Shelter was not found!");
            }
            return Ok(shelter);
        }

        [HttpPost]
        public async Task<IActionResult> AddShelterAsync([FromBody] Shelters shelter)
        {
            await _shelterService.AddShelterAsync(shelter);
            return Ok("Shelter was successfully added!");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateShelterAsync([FromBody] Shelters shelter)
        {
            await _shelterService.UpdateShelterAsync(shelter);
            return Ok("Shelter was successfully updated!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShelterAsync(Guid id)
        {
            await _shelterService.DeleteShelterAsync(id);
            return Ok("Shelter was successfully deleted!");
        }
    }
}
