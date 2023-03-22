using Discord;
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
        public async Task Test()
        {
            await RespondAsync($"You executed the test command!", ephemeral: true);
        }

        [SlashCommand("interact", "Clicky things!")]
        public async Task Interact()
        {
            var button = new ButtonBuilder()
            {
                Label = "Click me!",
                CustomId = "clickme",
                Style = ButtonStyle.Primary
            };

            var menu = new SelectMenuBuilder()
            {
                CustomId = "menu",
                Placeholder = "Expand me!"
            };

            menu.AddOption("Choose me!", "I can always count on you <3", isDefault: true);
            menu.AddOption("No, me!", "You won't regret it ;)");

            var component = new ComponentBuilder();
            component.WithButton(button);
            component.WithSelectMenu(menu);

            await RespondAsync("Here are some clicky things for you!", components: component.Build());
        }

        [SlashCommand("embed", "An embedded message")]
        public async Task Embed()
        {
            var embed = new EmbedBuilder()
            {
                Title = "Hello!",
                Description = "This is an embed.",
                Color = Color.Green
            };

            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }

        // ComponentInteraction("CustomId")
        [ComponentInteraction("clickme")]
        public async Task HandleButtonInput()
        {
            await RespondWithModalAsync<DemoModal>("demo_modal");
        }

        [ComponentInteraction("menu")]
        public async Task HandleMenuSelection(string[] inputs)
        {
            await RespondAsync(inputs[0], ephemeral: true);
        }

        [ModalInteraction("demo_modal")]
        public async Task HandleModalInput(DemoModal modal)
        {
            string input = modal.Input;
            await RespondAsync(input, ephemeral: true);
        }
    }

    public class DemoModal : IModal
    {
        public string Title => "Text Prompt";
        [InputLabel("Write something!")]
        [ModalTextInput("text_input", TextInputStyle.Short, placeholder: "Be silly!", maxLength: 30)]

        public string Input { get; set; }
    }
}