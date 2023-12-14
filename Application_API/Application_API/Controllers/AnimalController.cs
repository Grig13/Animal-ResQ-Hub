using Application_API.Models;
using Application_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Application_API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AnimalController : Controller
    {
        private readonly IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpGet("all-animals")]
        public async Task<IActionResult> GetAllAnimalsAsync()
        {
            var animals = await _animalService.GetAllAnimalsAsync();
            return Ok(animals);
        }

        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs()
        {
            var dogs = await _animalService.GetDogs();
            return Ok(dogs);
        }

        [HttpGet("cats")]
        public async Task<IActionResult> GetCats()
        {
            var cats = await _animalService.GetCats();
            return Ok(cats);
        }

        [HttpGet("animal-by-id")]
        public async Task<IActionResult> GetAnimalByIdAsync(Guid id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return StatusCode(404, "Animal was not found!");
            }
            return Ok(animal);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimalAsync([FromBody] Animals animal)
        {
            await _animalService.AddAnimalAsync(animal);
            return Ok("Animal was successfully added!");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateAnimalAsync([FromBody] Animals animal)
        {
            await _animalService.UpdateAnimalAsync(animal);
            return Ok("Animal was successfully updated!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimalAsync(Guid id)
        {
            await _animalService.DeleteAnimalAsync(id);
            return Ok("Animal was successfully deleted!");
        }
    }
}
