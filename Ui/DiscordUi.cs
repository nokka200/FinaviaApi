using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using System.Text;
using Finaviaapi.Http;
using Finaviaapi.Flight;
using Finaviaapi.Files;

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
        Flights? flightObj;
        ApiConnector apiObj;
        const string BASE_URI = "https://api.finavia.fi/flights/public/v0/flights/";
        const string APP_ID = "FINAVIA_APP_ID";
        const string APP_KEY = "FINAVIA_APP_KEY";


        // properties
        public string? AppToken { get;} // Bot token is important
        public DiscordSocketClient Client { get; }
        public int RefreshInterval { get; set; } = 20000;
        public int HourDifference { get; set; } = 24;
        public string FileName { get; }

        public DiscordUi(string appToken, string fileName)
        {
            AppToken = Environment.GetEnvironmentVariable(appToken);
            apiObj = new(BASE_URI, APP_ID, APP_KEY);
            Client = new();
            FileName = fileName;
        }

        // public methods
        /// <summary>
        /// Sets up and starts the bot
        /// </summary>
        /// <returns></returns>
        public async Task StartApp()
        {
            Client.Log += Log;                                      // Assign logging
            Client.Ready += ClientReadyAsync;                       // assign slash commands
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
                DebugPrint($"User {arg.Author.Username} {arg}");   
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
            //await arg.RespondAsync($"You executed {arg.Data.Name}");

            // Gives did not respond error
            while(true) 
            {
                await arg.Channel.SendMessageAsync("Spuha");
                //await Task.Delay(RefreshInterval);
                Thread.Sleep(RefreshInterval);
            }
        }

        private async Task<string> CreateAndUpdateFlightData(int airport)
        {
            // Used to get new data and format a string

            var re = await apiObj.GetArrivalStrAsync(airport);
            WriteToFile.Write(FileName, ".xml", re);

            return string.Empty;
        }

    }

}
