//Aqui ondem vamos trata sobre o level
using Bot.Service;
//Global
using Bot.GlobalVar;
//Type
using Bot.Types;
//utils
using Bot.Utils;
//Gerar imagem
using Bot.GerarImagemBrowser;
//Outros
using System.Text.Json;
using Discord;
using Discord.WebSocket;

namespace Bot.Funcao.levelUp
{
    class LevelUp()
    {
        //Id do canal, ondem fica as messagem de subiu de nivel/level
        private const ulong ID_ChannelLevel = 1445789541561663498;
        //caminho do video
        private const string PathVideoUp = @"videos\levelup.mp4";


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
            //percorrendo a lista do membros onlines
            foreach (var userid in membros_online)
            {
                //esse id tem no bando de dados?    
                if (Global.DicUserInforCall.ContainsKey(userid)) // estive vamos verificar.
                {
                    //informacoes do usuario
                    UserInforCall user = Global.DicUserInforCall[userid];
                    Console.WriteLine(JsonSerializer.Serialize(user));
                    //Pegando level e ms
                    int level = user.Level;
                    long ms = user.Ms;
                    // Agora para verificar se o usuario passou de level o bagulho é isso:
                    // level = 1
                    // ele tem 10000 = 10mil ms.
                    // para verificar se o usuario passou pro 2 vamos fazer:
                    // level * 10000 (1 * 10000) 10000ms = 10m
                    // se ms for maior que a conta de cima significa que ele passou de level.
                    if (ms >= (level * 100000)) // ele  passou de level
                    {
                        //Add +level
                        Global.DicUserInforCall[userid].Level++;
                        //E coloca o valor no Ms(ondem o mesmo é usada para guardar o ponto para subir de nivel)
                        Global.DicUserInforCall[userid].Ms = 0;
                        //mandando a messagem
                        await MandaMsgCanalLevel(userid);
                        continue;
                    }
                }
                else continue;
            }
            //depois do loop vamos salvar no json
            await UtilsFN.Escrevercallinfojson();
        }

        private static async Task MandaMsgCanalLevel(ulong userid)
        {
            //Pegando o canal
            var canal = await BotServiceClient.Client.GetChannelAsync(ID_ChannelLevel) as IMessageChannel;
            //Pegando o usuario
            var user = await BotServiceClient.Client.GetUserAsync(userid);
            //level
            int level = Global.DicUserInforCall[userid].Level;
            //Pegando o caminho da imagem
            string path = await GerarImagem.SendMessagemLevelUp(Global.DicUserInforCall[userid]);
            //Mandando messagem
            await canal!.SendFileAsync(path, user.Mention);
            //Deletando a imagem usada
            _ = Task.Run(() => File.Delete(path));

            //fds
            //10% de chance para ele manda um nigga
            if (new Random().Next(1, 100) <= 10) await canal.SendFileAsync(PathVideoUp, user.Mention +" Nigga\n");
        }
    }
}