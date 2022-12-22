using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CullingBot.Modules
{
    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("test", "This is a command built with a command module for testing purposes")]
        public async Task NewTest()
        {
            await RespondAsync($"You executed the command!", ephemeral: true);
        }
    }
}