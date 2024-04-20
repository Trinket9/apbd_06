using apbd_06.Models;
using apbd_06.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_06.Controllers;

[ApiController]
[Route("api/animals")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;
    public AnimalController(IAnimalService animalService) {
        _animalService = animalService;
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, Animal updatedAnimal) {
        _animalService.UpdateAnimal(idAnimal, updatedAnimal);
        return NoContent();
    }
}