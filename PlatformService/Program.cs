using Microservices.RabbitMQ.Extensions;
using Microservices.RabbitMQ.Types;
using Microsoft.EntityFrameworkCore;
using PlatformService.Core.Interfaces.Http;
using PlatformService.Core.Services.Http;
using PlatformService.Data;
using PlatformService.Data.Interfaces;
using PlatformService.Data.Repositories;
using PlatformService.Data.Seed;

namespace PlatformService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

            var app = builder.Build();
            
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            
            PlatformSeed.SeedPlatforms(app, app.Environment.IsProduction());

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            ConfigureRabbitMqServices(services, configuration);
            
            services.AddScoped<IPlatformRepository, PlatformRepository>();

            if (environment.IsProduction())
            {
                Console.WriteLine("-> Using PGSQL");
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("PlatformService_PGSQL")));
            }
            else
            {
                Console.WriteLine("-> Using InMemory db");
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("PlatformService"));
            }

            Console.WriteLine($"--> Command Service Endpoint: {configuration["Dependencies:CommandsService:Platforms"]}.");
            Console.WriteLine($"--> RabbitMQ Service: {configuration["Dependencies:RabbitMQ:Host"]}:{configuration["Dependencies:RabbitMQ:Port"]}.");
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

            services.AddPlatformMessageBusProducer(new MessageBusOptions
            {
                ExchangeName = platformsExchangeName,
                QueueName = platformsQueueName,
                QueueAndExchangeRoutingKey = platformsQueueAndExchangeRoutingKey
            });
            //Configure other related queues here
            #endregion
        }
    }
}