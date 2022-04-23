namespace MongoDb.Data
{
    public interface IMongoRepository
    {
        Task<TOut> GetById<TOut>(string table, Guid id);

        Task<List<TOut>> Select<TOut>(string table);

        Task Insert<TOut>(string table, TOut record);

        // Insert or Update
        Task Upsert<TOut>(string table, Guid id, TOut record); 

        Task Delete<TOut>(string table, Guid id);
    }
}
