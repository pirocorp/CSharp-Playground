namespace MongoDb.Services;

using Data.Models;

public interface IUsersService
{
    Task<T> GetById<T>(string guid);

    Task<T> GetById<T>(Guid guid);

    Task<IEnumerable<Person>> GetAll();

    Task<IEnumerable<T>> Select<T>();

    Task Create(
        string firstName,
        string lastName,
        string street,
        string city,
        string state, 
        string zip,
        string guid = null);

    Task Upsert(string guid, Person person);

    Task Upsert(Guid guid, Person person);

    Task Delete(Guid guid);
}