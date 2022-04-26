namespace MongoDb.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    public class PersonModel
    {
        [BsonId] // _id in MongoDB
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [BsonElement("dob")] // In database will be stored as dob
        public DateTime DateOfBirth { get; set; }

        public AddressModel PrimaryAddress { get; set; }
    }
}
