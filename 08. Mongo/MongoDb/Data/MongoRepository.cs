namespace MongoDb.Data
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoDatabase db;

        public MongoRepository(string database)
        {
            var client = new MongoClient(); // default connects to localhost
            db = client.GetDatabase(database);
        }

        public async Task<TOut> GetById<TOut>(string table, Guid id)
        {
            var collection = this.db.GetCollection<TOut>(table);
            var filter = Builders<TOut>.Filter.Eq("Id", id);

            return (await collection.FindAsync<TOut>(filter)).First();
        }

        public async Task<List<TOut>> Select<TOut>(string table)
        {
            var collection = db.GetCollection<TOut>(table);

            var response = await collection.FindAsync(new BsonDocument());

            return await response.ToListAsync();
        }

        public async Task Insert<TOut>(string table, TOut record)
        {
            var collection = this.db.GetCollection<TOut>(table);

            await collection.InsertOneAsync(record);
        }

        public async Task Upsert<TOut>(string table, Guid id, TOut record) // Insert or Update
        {
            var collection = this.db.GetCollection<TOut>(table);

            var guid = new BsonBinaryData(id, GuidRepresentation.Standard);
            var filter = new BsonDocument("_id", guid);

            await collection.ReplaceOneAsync(
                filter, 
                record, 
                new ReplaceOptions(){ IsUpsert = true });
        }

        public async Task Delete<TOut>(string table, Guid id)
        {
            var collection = this.db.GetCollection<TOut>(table);
            var filter = Builders<TOut>.Filter.Eq("Id", id);

            await collection.DeleteOneAsync(filter);
        }
    }
}
