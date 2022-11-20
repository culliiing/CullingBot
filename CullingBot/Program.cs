using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using Discord.Net;
using Newtonsoft.Json;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();

    private DiscordSocketClient client;

    public async Task MainAsync()
    {
        client = new DiscordSocketClient();
        client.Log += Log;
        client.Ready += Client_Ready;
        client.SlashCommandExecuted += SlashCommandHandler;

        string token = File.ReadAllText(Environment.CurrentDirectory + "\\..\\..\\..\\token.txt");

        // I don't know why I can't get env to work.
        //Console.WriteLine(Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
        //Console.ReadKey();
        //var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        //var guildID = Environment.GetEnvironmentVariable("GUILD_ID");

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public async Task Client_Ready()
    {
        // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
        //var guild = client.GetGuild(guildId);

        // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
        //var guildCommand = new SlashCommandBuilder();

        // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
        //guildCommand.WithName("first-command");

        // Descriptions can have a max length of 100.
        //guildCommand.WithDescription("This is my first guild slash command!");

        // Let's do our global command
        var globalCommand = new SlashCommandBuilder();
        globalCommand.WithName("Button");
        globalCommand.WithDescription("This will generate a pressable button.");

        try
        {
            // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
            //await guild.CreateApplicationCommandAsync(guildCommand.Build());

            // With global commands we don't need the guild.
            await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
            // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
        }
        catch (ApplicationCommandException exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        await command.RespondAsync($"You executed {command.Data.Name}");
    }
}