using Discord.WebSocket;

namespace Bot.Service
{
    class BotServiceClient
    {
        public static DiscordSocketClient Client { get; private set; }
        
        public static void Init(DiscordSocketClient client)
        {
            Client = client;
        }
    }
}