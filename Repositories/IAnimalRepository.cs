using apbd_06.Models;

namespace apbd_06.Repositories;

public interface IAnimalRepository
{
    public void UpdateAnimal(int idAnimal, Animal animal);
}