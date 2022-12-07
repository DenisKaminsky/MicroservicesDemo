using CommandsService.Core.Interfaces.Grpc;
using CommandsService.Core.Services.Grpc;
using CommandsService.Data;
using CommandsService.Data.Interfaces;
using CommandsService.Data.Repositories;
using CommandsService.Data.Seed;
using Microservices.RabbitMQ.Extensions;
using Microservices.RabbitMQ.Types;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CommandsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Seed database on App start
            PlatformSeed.SeedPlatforms(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            ConfigureRabbitMqServices(services, configuration);
            ConfigureDbContext(services);
            
            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<ICommandRepository, CommandRepository>();
            services.AddScoped<IPlatformDataClient, PlatformDataClient>();
        }

        private static void ConfigureRabbitMqServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddConnectionFactory(new MessageBusServerOptions
            {
                Host = configuration["Dependencies:RabbitMQ:Host"],
                Port = int.Parse(configuration["Dependencies:RabbitMQ:Port"])
            });

            #region Platforms
            var platformsExchangeName = configuration["Dependencies:RabbitMQ:Platforms:ExchangeName"];
            var platformsQueueName = configuration["Dependencies:RabbitMQ:Platforms:QueueName"];
            var platformsQueueAndExchangeRoutingKey = configuration["Dependencies:RabbitMQ:Platforms:QueueAndExchangeRoutingKey"];

            services.AddPlatformMessageBusConsumer(new MessageBusOptions
            {
                ExchangeName = platformsExchangeName,
                QueueName = platformsQueueName,
                QueueAndExchangeRoutingKey = platformsQueueAndExchangeRoutingKey
            });
            #endregion
        }

        private static void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("CommandsService"));
        }
    }
}