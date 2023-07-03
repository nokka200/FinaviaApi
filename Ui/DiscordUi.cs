using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using System.Text;
using Finaviaapi.Http;
using Finaviaapi.Flight;
using Finaviaapi.Files;
using Finaviaapi.Serializer;

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
        public int RefreshInterval { get; set; } = 90000;
        public int HourDifference { get; set; } = 24;
        public string FileName { get; }
        public int Airport { get;} = 3;

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
            DateTime arrival;

            // Gives did not respond error
            while (true) 
            {
                
                //await Task.Delay(RefreshInterval);
                await CreateFlightDataAsync(Airport);
                UpdateDataLocal();

                foreach(var item in flightObj.arr.flight)
                {
                    DateTime.TryParse(item.sdt, out arrival);

                    if (arrival.Date == DateTime.Now.Date && arrival.Hour < (DateTime.Now.Hour + HourDifference))
                        await arg.Channel.SendMessageAsync(FormatFlight(item));   
                }
                await arg.Channel.SendMessageAsync("---------------");
                Thread.Sleep(RefreshInterval);
            }
        }

        private async Task CreateFlightDataAsync(int airport)
        {
            // Used to get new data and format a string

            var re = await apiObj.GetArrivalStrAsync(airport);
            WriteToFile.Write(FileName, ".xml", re);
        }

        private void UpdateDataLocal()
        {
            // Updates the Flight object with new data from Current.xml
            flightObj = SerializeFlightData(FileName);
        }

        private Flights SerializeFlightData(string fileName)
        {
            // Gets the current working directory and serializes a Flight object from the information.
            string currentPath = Directory.GetCurrentDirectory();

            object tempObj = MyObjectSerializer.ReadObject(currentPath + "/" + fileName + ".xml", typeof(Flights));

            flightObj = (Flights)tempObj;
            return flightObj;
        }

        private string FormatFlight(flight item)
        {
            string re;
            // First compated estimated arrival time with arrival time, then prints a the information in a 
            // formated form.
            DateTime estArrival;
            DateTime arrivalTime;

            DateTime.TryParse(item.sdt, out arrivalTime);
            DateTime.TryParse(item.estD, out estArrival);

            // Methods that check stuff
            estArrival = CheckEstimate(item, estArrival, arrivalTime);

            re = $"Lennon numero:\t {item.fltnr}, {item.cflight1} {item.cflight2}, {item.cflight3}, {item.cflight4}, {item.cflight5} {item.cflight6}\n" +
                $"Saapumisaika:\t {arrivalTime}\n" +
                $"Tila:\t\t {item.prtF}\n" +
                $"Arvioitu:\t {estArrival}";

            return re;
        }

        private static DateTime CheckEstimate(flight item, DateTime estArrival, DateTime arrivalTime)
        {
            if (item.estD == string.Empty)
            {
                estArrival = arrivalTime;
            }

            return estArrival;
        }

    }

}
