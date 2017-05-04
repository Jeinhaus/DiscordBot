using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Core.Commands
{
    public class HelpCommand : ModuleBase
    {
        // !echo hello -> hello
        [Command("echo"), Summary("Echos a message.")]
        public async Task Echo([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }

        // !help -> shows info about commands
        [Command("help"), Summary("Shows information about the available commands.")]
        [Alias("commands", "command", "-h")]
        public async Task Help()
        {
            string help = "The available commands are: help, echo, mmr. To use a command type !<command>.";
            await ReplyAsync(help);
        }
    }
}
