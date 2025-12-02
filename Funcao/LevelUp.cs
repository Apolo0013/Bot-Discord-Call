//Aqui ondem vamos trata sobre o level
using Bot.Service;
using Discord;
using Discord.WebSocket;
//Global
using Bot.GlobalVar;
//Type
using Bot.Types;
using System.Text.Json;

namespace Bot.Funcao.levelUp
{
    class LevelUp()
    {
        //essa funcao vai verificar todo os 
        public static async Task VerificarLevelUp()
        {
            //Antes de pegar essa caralhadas de membro vamos verificar se tem nego no banco de daddos
            if (Global.DicUserInforCall.Count == 0 || BotServiceClient.Client == null) return;
            var guild = BotServiceClient.Client.GetGuild(758039648625754125);
            //Esperando pegar todos
            await guild.DownloadUsersAsync();
            //Membros
            var membros = guild.Users;
            //Mebros onlines
            List<ulong> membros_online = membros
                .Where(user => user.Status != UserStatus.Offline)
                .Select(x => x.Id)
                .ToList();
            Console.WriteLine(membros_online.Count + " Total de nego on" );
            //percorrendo a lista do membros onlines
            foreach (var userid in membros_online)
            {
                Console.WriteLine(Global.DicUserInforCall.ContainsKey(userid));
                //esse id tem no bando de dados?    
                if (Global.DicUserInforCall.ContainsKey(userid)) // estive vamos verificar.
                {
                    //informacoes do usuario
                    UserInforCall user = Global.DicUserInforCall[userid];
                    Console.WriteLine(JsonSerializer.Serialize(user));
                    //Pegando level e ms
                    int level = user.Level;
                    long ms = user.MiliSegundos;
                    // Agora para verificar se o usuario passou de level o bagulho é isso:
                    // level = 1
                    // ele tem 10000 = 10mil ms.
                    // para verificar se o usuario passou pro 2 vamos fazer:
                    // level * 10000 (1 * 10000) 10000ms = 10m
                    // se ms for maior que a conta de cima significa que ele passou de level.
                    if (ms >= (level * 10000 )) // ele  passou de level
                    {
                        //Add +level
                        Global.DicUserInforCall[userid].Level++;
                        continue;
                    }
                }
                else continue;
            }
        }
    }
}