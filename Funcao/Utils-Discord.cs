//Class para o discord
//BotService
using Bot.Service;
using Discord;
using Discord.WebSocket;

namespace Bot.Funcao.Utils_Discord
{
    class UtilsDiscord
    {
        public static async Task SendMessagemDM(ulong userid, string text)
        {
            //Usuario
            IUser user = await BotServiceClient.Client.GetUserAsync(userid);
            //Pegando a DM do usuario
            var dm = await user.CreateDMChannelAsync();
            //Mandando a messagem
            await dm.SendFileAsync(@"imagens\foto.jpg", text);
        }
    }
}