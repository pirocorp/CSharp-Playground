namespace Coin
{
    using Coin.Grpc.Services;
    using Coin.Scheduler;

    using Coravel;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBlockChain>(new BlockChain());

            services.AddScheduler();
            services.AddTransient<BlockJob>();

            services.AddGrpc();
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // app.UseGrpcWeb(); // Must be added between UseRouting and UseEndpoints
            // app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true }); // A
            // app.UseCors(); // B
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<BlockchainService>();

                // endpoints.MapGrpcService<BlockchainService>().EnableGrpcWeb().RequireCors("AllowAll"); // C
                // endpoints.MapGrpcService<BlockchainService>().RequireCors("AllowAll"); // D
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        "Communication with gRPC endpoints" +
                        " must be made through a gRPC client.");
                });
            });
        }
    }
}
