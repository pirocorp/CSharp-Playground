namespace MongoDb.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class NameModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
