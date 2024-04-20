using System.Data.SqlClient;
using apbd_06.Models;
using apbd_06.Repositories;
using apbd_06.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/animals", (string? order, IConfiguration configuration) => {
    List<Animal> animals = new List<Animal>();
    using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
        string query = $"SELECT * FROM apbd_06.Animal ORDER BY Name ASC";;
        if (!string.IsNullOrEmpty(order)) {
            if (!new[] { "Name", "Description", "Category", "Area" }.Contains(order)) {
                return Results.BadRequest("bruh: incorrect ordering");
            } 
            query = $"SELECT * FROM apbd_06.Animal ORDER BY {order} ASC";
        }

        using var sqlCommand = new SqlCommand(query, sqlConnection);
        sqlCommand.Connection.Open();
        var rdr = sqlCommand.ExecuteReader();
        while (rdr.Read()) {
            animals.Add(new Animal {
                IdAnimal = rdr.GetInt32(0),
                Name = rdr.GetString(1),
                Description = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                Category = rdr.GetString(3),
                Area = rdr.GetString(4)
            });
        }
    }
    return Results.Ok(animals);
});

app.MapPost("api/animals", (Animal animal, IConfiguration configuration) => {
    using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
        var query = "INSERT INTO apbd_06.Animal (Name, Description, Category, Area) VALUES(@Name, @Description, @Category, @Area);";
        using var sqlCommand = new SqlCommand(query, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@Name", animal.Name);
        sqlCommand.Parameters.AddWithValue("@Category", animal.Category);
        sqlCommand.Parameters.AddWithValue("@Area", animal.Area);
        if (string.IsNullOrEmpty(animal.Description)) {
            sqlCommand.Parameters.AddWithValue("@Description", DBNull.Value); 
        } else {
            sqlCommand.Parameters.AddWithValue("@Description", animal.Description);
        }
        sqlCommand.Connection.Open();
        sqlCommand.ExecuteNonQuery();
    }

    return Results.Created("", null);
});

app.MapDelete("api/animals/{id}", (int idAnimal, IConfiguration configuration) => {
    using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"))) {
        var query = "DELETE FROM apbd_06.Animal WHERE idAnimal = @idAnimal";
        using var sqlCommand = new SqlCommand(query, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@idAnimal", idAnimal);
        sqlCommand.Connection.Open();
        sqlCommand.ExecuteNonQuery();
    }

    return Results.NoContent();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();