using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace CullingBot
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient client;
        private readonly InteractionService commands;
        private readonly IServiceProvider services;

        public InteractionHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
        {
            this.client = client;
            this.commands = commands;
            this.services = services;
        }

        public async Task InitializeAsync()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            client.InteractionCreated += HandleInteraction;
        }

        private async Task HandleInteraction(SocketInteraction argument)
        {
            try
            {
                var context = new SocketInteractionContext(client, argument);
                await commands.ExecuteCommandAsync(context, services);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
