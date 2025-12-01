using DotNetEnv;
//BOt main
using Bot.Main;


class Program
{
    public static async Task Main()
    {
        //Token
        Env.Load();
        string? tokenenv = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

        if (tokenenv == null) return; // caso ele nao exista
        string token = tokenenv;
        //Iniciando Bot
        BotRun Bot = new BotRun();
        await Bot.Run(token);
    }
}