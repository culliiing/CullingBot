using Discord;
using Discord.WebSocket;
using Discord.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;
using Discord.Interactions;
using CullingBot;
using Discord.Commands;

public class Program
{
    private DiscordSocketClient client;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        var config = new ConfigurationBuilder()
            // this will be used more later on
            .SetBasePath(AppContext.BaseDirectory)
            // I chose using YML files for my config data as I am familiar with them
            .AddYamlFile("config.yml")
            .Build();

        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
        services
        .AddSingleton(config)
        .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged,
            AlwaysDownloadUsers = true,
        }))
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
        .AddSingleton<InteractionHandler>()
        .AddSingleton(x => new CommandService())
        )
        .Build();

        await RunAsync(host);
    }

    public async Task RunAsync(IHost host)
    {
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;

        var commands = provider.GetRequiredService<InteractionService>();
        client = provider.GetRequiredService<DiscordSocketClient>();
        var config = provider.GetRequiredService<IConfigurationRoot>();

        await provider.GetRequiredService<InteractionHandler>().InitializeAsync();

        client.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
        commands.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };

        client.Ready += async () =>
        {
            Console.WriteLine("Bot ready!");
            await commands.RegisterCommandsToGuildAsync(UInt64.Parse(config["testGuild"]), true);
        };

        await client.LoginAsync(TokenType.Bot, config["tokens:discord"]);
        await client.StartAsync();

        await Task.Delay(-1);
    }
}