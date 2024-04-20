using apbd_06.Models;

namespace apbd_06.Services;

public interface IAnimalService
{
    public void UpdateAnimal(int idAnimal, Animal animal);
}