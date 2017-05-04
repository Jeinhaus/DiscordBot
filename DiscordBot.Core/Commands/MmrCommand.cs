using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Core.Commands
{
    public class MMRCommand : ModuleBase
    {
        // !mmr
        [Command("mmr"), Summary("Posts your current group and solo mmr.")]
        public async Task MMR()
        {
            await ReplyAsync("over 9000");
        }
    }
}
