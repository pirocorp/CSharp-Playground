namespace MongoDb
{
    using MongoDb.Data;
    using MongoDb.Data.Models;
    using MongoDb.Models;
    using MongoDb.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        private static readonly Guid Guid = new("e6e0be1e-fe8d-4a5f-b45e-69a7e7033851");
        private static IUsersService service;

        public static async Task Main()
        {
            service = BuildUsersService();

            // await CreateNewPerson();

            await GetAll();
            
            // await GetById();

            // await Upsert();

            // await Select();

            // await Delete();
        }

        private static async Task CreateNewPerson()
            => await service.Create(
                "Jane",
                "Doe",
                "501 Second Street",
                "New York",
                "New York",
                "10027",
                Guid.ToString());

        private static async Task GetAll()
        {
            var records = await service.GetAll();

            foreach (var record in records)
            {
                PrintPerson(record);
            }
        }

        private static async Task Select()
        {
            var records = await service.Select<NameModel>();

            foreach (var record in records)
            {
                Console.WriteLine($"{record.FirstName} {record.LastName}");
                Console.WriteLine();
            }
        }

        private static async Task GetById()
        {
            var record = await service.GetById<Person>(Guid);

            PrintPerson(record);
        }

        private static async Task Upsert()
        {
            var record = await service.GetById<Person>(Guid);
            record.DateOfBirth = new DateTime(2003, 5, 5, 0, 0, 0, DateTimeKind.Utc);

            await service.Upsert(Guid, record);
        }

        private static async Task Delete()
        {
            await service.Delete(Guid);
        }

        private static void PrintPerson(Person person)
        {
            Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}");

            if (person.PrimaryAddress is not null)
            {
                Console.WriteLine(person.PrimaryAddress.City);
            }

            Console.WriteLine();
        }
        
        private static IUsersService BuildUsersService()
            => new ServiceCollection()
                .AddSingleton<IMongoRepository, MongoRepository>(s => new MongoRepository("AddressBook"))
                .AddTransient<IUsersService, UsersService>()
                .BuildServiceProvider()
                .GetRequiredService<IUsersService>();
    }
}
