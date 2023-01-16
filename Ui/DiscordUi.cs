using Discord;
using Discord.WebSocket;

namespace FinaviaApi.Ui
{
    /// <summary>
    /// Discordbot ui class, used to make call's to finavia api and displau results
    /// </summary>
    public class DiscordUi
    {
        // fields
        const string BOT_NAME = "Finavia";

        // properties
        public string? AppToken { get;} // Bot token is important
        public DiscordSocketClient Client { get; }

        public DiscordUi(string appToken)
        {
            AppToken = Environment.GetEnvironmentVariable(appToken);
            Client = new();
            
        }

        // public methods
        public async Task StartApp()
        {
            Client.Log += Log;

            await Client.LoginAsync(TokenType.Bot, AppToken);

            Client.MessageReceived += ClientOnMessageReceivedAsync;

            await Client.StartAsync();

            await Task.Delay(-1);

        }

        // private methods
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private static Task ClientOnMessageReceivedAsync(SocketMessage arg)
        {
            if (arg.Author.Username != BOT_NAME)
                arg.Channel.SendMessageAsync($"User '{arg.Author.Username}' successfully ran helloworld!");

            return Task.CompletedTask;
        }
    }

}
