using DotNetEnv;
//Bot main
using Bot.Main;
//Gerado de imagem
using Bot.GerarImagemBrowser;
using Bot.Funcao.levelUp;


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
                Console.WriteLine("loop");
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
        //Inciaciando o browser, para gerar as imagens
        await GerarImagem.Init();
        await GerarImagem.GerarImagemFN(new()
        {
            Nome = "FOdase",
            Level = 1,
            MiliSegundos = 80000
        });
        return;
        //Inciando o background
        StartBackGround();
        //Iniciando Bot
        BotRun Bot = new BotRun();
        await Bot.Run(token);
    }
}