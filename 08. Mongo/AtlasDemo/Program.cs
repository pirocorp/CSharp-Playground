namespace AtlasDemo
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public static class Program
    {
        public static void Main()
        {
            var connectionString =
                "mongodb+srv://m220student:m220password@"
                + "mflix.kgrhu.mongodb.net/test?retryWrites=true&w=majority";

            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("sample_mflix");
            var collection = db.GetCollection<BsonDocument>("movies");

            var result = collection.Find(new BsonDocument())
                .SortByDescending(m => m["runtime"])
                .Limit(10)
                .ToList();

            foreach (var r in result)
            {
                Console.WriteLine(r.GetValue("title"));
            }
        }
    }
}