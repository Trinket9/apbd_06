using System.Data.SqlClient;
using apbd_06.Models;

namespace apbd_06.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    public AnimalRepository(IConfiguration configuration) {
        _configuration = configuration;
    }

    public void UpdateAnimal(int idAnimal, Animal animal) {
        using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default"))) {
            var query = @"UPDATE apbd_06.Animal 
                          SET Name = @Name, Description = @Description, Category = @Category, Area = @Area
                          WHERE IdAnimal = @IdAnimal;";
            using var sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name", animal.Name);
            sqlCommand.Parameters.AddWithValue("@Description", animal.Description ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@Category", animal.Category);
            sqlCommand.Parameters.AddWithValue("@Area", animal.Area);
            sqlCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
        }
    }
}