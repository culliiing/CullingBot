using Discord;
using Discord.WebSocket;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();

    private DiscordSocketClient client;

    public async Task MainAsync()
    {
        client = new DiscordSocketClient();

        client.Log += Log;

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
        //var token = "token";

        // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
        var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        var guildID = Environment.GetEnvironmentVariable("GUILD_ID");
        //var token = File.ReadAllText(@"..\..\..\.env");
        // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

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
}