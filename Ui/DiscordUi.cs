using Discord;
using Discord.WebSocket;
using System.Diagnostics;

namespace FinaviaApi.Ui
{
    /// <summary>
    /// Discordbot ui class, used to make call's to finavia api and display results
    /// </summary>
    public class DiscordUi
    {
        // fields
        const string BOT_NAME = "Finavia";
        const string COMMAND_NAME = "first";
        const string COMMAND_DESCRIPTION = "This is my first global slash command";

        // properties
        public string? AppToken { get;} // Bot token is important
        public DiscordSocketClient Client { get; }

        public DiscordUi(string appToken)
        {
            AppToken = Environment.GetEnvironmentVariable(appToken);
            Client = new();
            
        }

        // public methods
        /// <summary>
        /// Sets up and starts the bot
        /// </summary>
        /// <returns></returns>
        public async Task StartApp()
        {
            Client.Log += Log;                  // Assing logging
            Client.Ready += ClientReadyAsync;   // assing slash commands
            Client.SlashCommandExecuted += SlashCommandHandlerAsync;

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
        private Task ClientOnMessageReceivedAsync(SocketMessage arg)
        {
            if (arg.Author.Username != BOT_NAME)
            {
                arg.Channel.SendMessageAsync($"User '{arg.Author.Username}' successfully ran helloworld!");
                DebugPrint($"User {arg.Author.Username} {arg.ToString()}");   
            }
                
            return Task.CompletedTask;
        }

        private void DebugPrint(string content)
        {
            // used only for logging and debugging

            DateTime timeNow = DateTime.Now;
            Console.WriteLine($"{timeNow} \t{content}");
        }

        private async Task ClientReadyAsync()
        {
            // Code copied from manual!

            // Build the command
            var globalCommand = new SlashCommandBuilder();

            // assing name and description
            globalCommand.WithName(COMMAND_NAME);
            globalCommand.WithDescription(COMMAND_DESCRIPTION);

            await Client.CreateGlobalApplicationCommandAsync(globalCommand.Build());

        }

        private async Task SlashCommandHandlerAsync(SocketSlashCommand arg)
        {
            await arg.RespondAsync($"You executed {arg.Data.Name}");
        }
    }

}
