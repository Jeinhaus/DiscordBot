using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Core
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly string _token;
        private readonly IDependencyMap _map = new DependencyMap();
        private readonly CommandService _commands = new CommandService();
        private readonly ILogger _logger;

        public Bot(string token, ILogger logger)
        {
            _token = token;
            _logger = logger;
            _client = new DiscordSocketClient();
        }

        public async Task Run()
        {
            _client.Log += Logger;
            await InitCommands();
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            _client.MessageReceived += HandleCommand;

            await Task.Delay(-1);
        }

        private async Task InitCommands()
        {
            // Repeat this for all the service classes
            // and other dependencies that your commands might need.
            //_map.Add(new SomeServiceClass());

            // Either search the program and add all Module classes that can be found:
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            // Or add Modules manually if you prefer to be a little more explicit:
            //await _commands.AddModuleAsync<SomeModule>();

            // Subscribe a handler to see if a message invokes a command.

        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;

            // Create a Command Context
            var context = new CommandContext(_client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed succesfully)
            var result = await _commands.ExecuteAsync(context, argPos, _map);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
        // Create a named logging handler, so it can be re-used by addons
        // that ask for a Func<LogMessage, Task>.
        private Task Logger(LogMessage message)
        {
            string msg = "{severity}, {message}, {source}";
            object[] param = { message.Severity, message.Message, message.Source };
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    _logger.LogCritical(msg, param);
                    break;
                case LogSeverity.Error:
                    _logger.LogError(msg, param);
                    break;
                case LogSeverity.Warning:
                    _logger.LogWarning(msg, param);
                    break;
                case LogSeverity.Info:
                    _logger.LogInformation(msg, param);
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    _logger.LogDebug(msg, param);
                    break;
            }

            return Task.CompletedTask;
        }

    }
}
