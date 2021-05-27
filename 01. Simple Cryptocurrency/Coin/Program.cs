namespace Coin
{
    using Coin.Scheduler;

    // Used for Scheduling Tasks
    using Coravel;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public static class Program
    {
        public static void Main()
        {
            var host = CreateHostBuilder().Build();
            host.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<BlockJob>()
                    .EveryThirtySeconds();
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder()
            => Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
