using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CullingBot.Modules
{
    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region Interact
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

            menu.AddOption("Choose me!", "I can always count on you <3");
            menu.AddOption("No, me!", "You won't regret it ;)");

            var component = new ComponentBuilder();
            component.WithButton(button);
            component.WithSelectMenu(menu);

            await RespondAsync("Here are some clicky things for you!", components: component.Build());
        }

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
        #endregion

        #region Embed
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
        #endregion

        [SlashCommand("poll", "Create a poll")]
        public async Task HandlePoll(string question)
        {
            var title = question;
            var poll = Poll.GetNewPoll(title);

            var yesButton = new ButtonBuilder()
            {
                Label = "Yes",
                // Thumbs-up emoji
                //Emote = new Emoji(@"\uD83D\uDC4D"),
                CustomId = $"yes:{poll.Id}",
                Style = ButtonStyle.Success
            };

            var noButton = new ButtonBuilder()
            {
                Label = "No",
                // Thumbs-down emoji
                //Emote = new Emoji(@"\uD83D\uDC4E"),
                CustomId = $"no:{poll.Id}",
                Style = ButtonStyle.Danger
            };

            var component = new ComponentBuilder();
            component.WithButton(yesButton);
            component.WithButton(noButton);

            await RespondAsync(embed: Poll.GetPollEmbed(poll.Id), components: component.Build());
        }

        [ComponentInteraction("yes:*")]
        public async Task YesButton(int pollId)
        {
            await (Context.Interaction as IComponentInteraction).UpdateAsync(x =>
            {
                x.Embed = Poll.GetPollEmbed(pollId, true);
            });
        }

        [ComponentInteraction("no:*")]
        public async Task NoButton(int pollId)
        {
            await (Context.Interaction as IComponentInteraction).UpdateAsync(x =>
            {
                x.Embed = Poll.GetPollEmbed(pollId, false);
            });
        }
    }

    public class DemoModal : IModal
    {
        public string Title => "Text Prompt";
        [InputLabel("Write something!")]
        [ModalTextInput("text_input", TextInputStyle.Short, placeholder: "Be silly!", maxLength: 32)]
        
        public string Input { get; set; }
    }
}