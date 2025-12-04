using DotNetEnv;
//Bot main
using Bot.Main;
//Gerado de imagem
using Bot.GerarImagemBrowser;
//Outros
using Bot.Funcao.levelUp;
//Global de variavels
using Bot.GlobalVar;
//Utils
using Bot.Utils;
//Type
using Bot.Types;
using System.Text.Json;

class Program
{
    //loop que vai fica rodando enquanto o bot esta vivo.
    public static void StartBackGround()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                //Chamando verificado
                await LevelUp.VerificarLevelUp();
                await Task.Delay(5000);
            }
        });
    }

    public static async Task Main()
    {
        //Token
        Env.Load();
        string? tokenenv = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        if (tokenenv == null) return; // caso ele nao exista
        string token = tokenenv;
        //=====================================================
        //Inciaciando o browser, para gerar as imagens
        await GerarImagem.Init();
        //Inciando o background
        StartBackGround();
        //=====================================================
        //Pegando o banco de dados
        Dictionary<ulong, UserInforCall>? dados = await UtilsFN.Lercallinfojson()!;
        //Caso ele retorne null, que dizer que algo de errado tem no json
        if (dados == null)
        {
            Console.WriteLine("O bot nao pode se inicializado sem banco de dados, algo deu ruim em pegar.");
            return;
        }
        //Mandando esse valor la pra variavel global.
        Global.DicUserInforCall = dados;
        //=====================================================
        //Iniciando Bot
        BotRun Bot = new BotRun();
        await Bot.Run(token);
    }
}