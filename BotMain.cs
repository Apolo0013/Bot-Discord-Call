using Discord;
using Discord.WebSocket;
//Funcao
using Bot.Funcao.CallCapture;
using Bot.GerarImagemBrowser;
//Bot service
using Bot.Service;
using Bot.GlobalVar;
//Utils
using Bot.Funcao.Utils_Discord;


namespace Bot.Main
{
    public class BotRun
    {
        public static DiscordSocketClient? _client;

        public async Task Run(string token)
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents =
                    GatewayIntents.Guilds |
                    GatewayIntents.GuildMessages |
                    GatewayIntents.GuildMembers |
                    GatewayIntents.MessageContent |
                    GatewayIntents.GuildPresences |
                    GatewayIntents.GuildVoiceStates   // ← NECESSÁRIO pra detectar call
            };

            _client = new DiscordSocketClient(config);

            _client.Log += msg =>
            {
                Console.WriteLine(msg);
                return Task.CompletedTask;
            };

            _client.MessageReceived += OnMessage;
            _client.UserVoiceStateUpdated += OnVoiceStateUpdated;    // ← EVENTO DA CALL

            string Token = token;

            await _client.LoginAsync(TokenType.Bot, Token);
            await _client.StartAsync();
            //Pegando o client
            BotServiceClient.Init(_client);

            await Task.Delay(-1);
        }

        private async Task OnMessage(SocketMessage msg)
        {
            //Cmd
            string cmd = msg.Content.Split(" ")[0];
            if (cmd == "") return;
            //CMDS
            if (cmd == "!show")
            {
                _ = Task.Run(async () =>
                {
                    //Verificando se esta registrado ou nao, para registrar e fala se ele estava ou nao.
                    bool registrado = await UtilsDiscord.VerificarRegistror(msg.Author);
                    //se ele nao esteve registrado...
                    if (!registrado) await msg.Channel.SendMessageAsync($"{msg.Author.Mention} Registrando...");
                    //segue normal
                    //Pegando o caminho da imagem.
                    string path = await GerarImagem.SendMessagemShowInfo(Global.DicUserInforCall[msg.Author.Id]);
                    //mandando a imagem
                    await msg.Channel.SendFileAsync(path, msg.Author.Mention);
                    //Deletando a imagem
                    _ = Task.Run(() => File.Delete(path)); // delentando o arquivo asicrono
                });
            }
        }

        private async Task OnVoiceStateUpdated(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            // Entrou na call
            if (before.VoiceChannel == null && after.VoiceChannel != null) await CallCapture.EntrarCall(user, after.VoiceChannel.Name);

            // Saiu da call
            else if (before.VoiceChannel != null && after.VoiceChannel == null) await CallCapture.SairCall(user.Id);

            await Task.CompletedTask;
        }
    }
}