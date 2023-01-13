using DSharpPlus;
using DSharpPlus.Entities;

namespace FinaviaApi.Ui
{
    /// <summary>
    /// Discordbot ui class, used to make call's to finavia api and displau results
    /// </summary>
    public class DiscordUi
    {
        // fields
        

        // properties
        public string AppId { get;}
        public string AppKEy { get;}
        public string AppToken { get;} // Bot token is important
        public DiscordClient DiscordBotObj { get;}

        public DiscordUi(string appId, string appKey, string appToken)
        {
            AppId = appId;
            AppKEy = appKey;
            AppToken = appToken;

            DiscordBotObj = new DiscordClient(new DiscordConfiguration()
            {
                Token = Environment.GetEnvironmentVariable(AppToken),
                TokenType = TokenType.Bot,
            });
        }

        public void EchoOnMessage()
        {
            DiscordBotObj.MessageCreated += async (client, args) =>
            {
                if(args.Message.Content.StartsWith("ping"))
                {
                    await client.SendMessageAsync(args.Channel, "ping");
                }
            };
        }
       
    }
}
