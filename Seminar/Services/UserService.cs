using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Seminar.Models;

namespace Seminar.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(
            IOptions<SeminarDatabaseSettings> seminarDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                seminarDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                seminarDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                seminarDatabaseSettings.Value.UserCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }


public static class UserEndpoints
{
	public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/User", () =>
        {
            return new [] { new User() };
        })
        .WithName("GetAllUsers")
        .Produces<User[]>(StatusCodes.Status200OK);

        routes.MapGet("/api/User/{id}", (int id) =>
        {
            //return new User { ID = id };
        })
        .WithName("GetUserById")
        .Produces<User>(StatusCodes.Status200OK);

        routes.MapPut("/api/User/{id}", (int id, User input) =>
        {
            return Results.NoContent();
        })
        .WithName("UpdateUser")
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/User/", (User model) =>
        {
            //return Results.Created($"//api/Users/{model.ID}", model);
        })
        .WithName("CreateUser")
        .Produces<User>(StatusCodes.Status201Created);

        routes.MapDelete("/api/User/{id}", (int id) =>
        {
            //return Results.Ok(new User { ID = id });
        })
        .WithName("DeleteUser")
        .Produces<User>(StatusCodes.Status200OK);
    }
}}
