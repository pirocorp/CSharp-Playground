namespace MongoDb.Services
{
    using Data;
    using Data.Models;
    
    public class UsersService : IUsersService
    {
        private const string Table = "Users";
        private readonly IMongoRepository db;

        public UsersService(IMongoRepository db)
        {
            this.db = db;
        }

        public async Task<T> GetById<T>(string guid)
            => await this.GetById<T>(new Guid(guid));

        public async Task<T> GetById<T>(Guid guid)
            => await db.GetById<T>(Table, guid);

        public async Task<IEnumerable<T>> GetAll<T>()
            => await db.Select<T>(Table);

        public async Task<IEnumerable<T>> Select<T>()
            => await db.Select<T>(Table);

        public async Task Create(
            string firstName,
            string lastName,
            string street,
            string city,
            string state, 
            string zip,
            string guid = null)
        {
            var person = new Person()
            {
                FirstName = firstName,
                LastName = lastName,
                PrimaryAddress = new Address()
                {
                    StreetAddress = street,
                    City = city,
                    State = state,
                    ZipCode = zip
                }
            };

            if (guid != null)
            {
                person.Id = new Guid(guid);
            }

            await db.Insert(Table, person);
        }

        public async Task Upsert(
            string guid, 
            string firstName,
            string lastName,
            string street,
            string city,
            string state, 
            string zip)
            => await this.Upsert(
                new Guid(guid), 
                firstName,
                lastName,
                street,
                city,
                state, 
                zip);

        public async Task Upsert(
            Guid guid,             
            string firstName,
            string lastName,
            string street,
            string city,
            string state, 
            string zip)
        {
            var person = new Person()
            {
                Id = guid,
                FirstName = firstName,
                LastName = lastName,
                PrimaryAddress = new Address()
                {
                    StreetAddress = street,
                    City = city,
                    State = state,
                    ZipCode = zip
                }
            };

            await this.db.Upsert(Table, guid, person);
        }

        public async Task Delete(Guid guid)
            => await this.db.Delete<Person>(Table, guid);
    }
}
