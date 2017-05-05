using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Core
{
    class Program
    {
        private static ILogger _logger;
        public static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            _logger =  serviceProvider.GetService<ILoggerFactory>().CreateLogger("program");

            var bot = serviceProvider.GetService<Bot>();

            try
            {
                bot.Run().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Failed to Run() the bot...", ex);
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            services.AddSingleton(loggerFactory);
            services.AddLogging();

            IConfigurationRoot configuration = GetConfiguration();
            services.AddSingleton<IConfigurationRoot>(configuration);
            services.AddOptions();
            services.AddSingleton(provider => new Bot(configuration["DISCORD_BOTTOKEN"], loggerFactory.CreateLogger("bot")));
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }


    }

}

