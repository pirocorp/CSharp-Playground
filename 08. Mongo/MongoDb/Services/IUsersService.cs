namespace MongoDb.Services
{
    public interface IUsersService
    {
        Task<TOut> GetById<TOut>(string guid);

        Task<TOut> GetById<TOut>(Guid guid);

        Task<IEnumerable<TOut>> GetAll<TOut>();

        Task<IEnumerable<TOut>> Select<TOut>();

        Task Create(
            string firstName,
            string lastName,
            string street,
            string city,
            string state, 
            string zip,
            string guid = null);

        Task Upsert(
            string guid, 
            string firstName,
            string lastName,
            string street,
            string city,
            string state, 
            string zip);

        Task Upsert(
            Guid guid, 
            string firstName,
            string lastName,
            string street,
            string city,
            string state, 
            string zip);

        Task Delete(Guid guid);
    }
}
