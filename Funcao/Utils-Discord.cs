//Class para o discord
//BotService
using Bot.Service;
using Discord;
using Discord.WebSocket;
//Global
using Bot.GlobalVar;
//Type
using Bot.Types;

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

        //Funcao que vai verificar se um cara esta registrado no banco de dados ou nao, senao ele criar, retorna true ou false, falando se o mesmo ja estava ou nao registrado.
        public static async Task<bool> VerificarRegistror(SocketUser user)
        {
            //Id 
            ulong userid = user.Id;
            if (!Global.DicUserInforCall.ContainsKey(userid))
            {
                Global.DicUserInforCall.Add(userid, new UserInforCall()
                {
                    NomeGlobal = user.GlobalName, // nome Global
                    Nome = user.Username, // nome que ele usar.
                    Ms = 0, // o tempo
                    TotalMs = 0, // aqui fica o total, a soma de tudo
                    Level = 1, // comeca no level 1
                    ID = userid
                });
                return false; // estava registrado
            }
            return true; // ja esta registrado
        }
    }
}