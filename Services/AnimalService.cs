using apbd_06.Models;
using apbd_06.Repositories;

namespace apbd_06.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    
    public AnimalService(IAnimalRepository animalRepository) {
        _animalRepository = animalRepository;
    }

    public void UpdateAnimal(int idAnimal, Animal animal) {
        _animalRepository.UpdateAnimal(idAnimal,animal);
    }
}