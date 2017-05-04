using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Core.Commands
{
    public class GitCommands : ModuleBase
    {
        public GitCommands()
        {

        }

        [Command("git"), Summary("Prints the last commits.")]
        public async Task Git()
        {
            await ReplyAsync("nothing yet.");
        }
    }
}
