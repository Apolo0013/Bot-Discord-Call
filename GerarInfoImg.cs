using Bot.Funcao.levelUp;
using Microsoft.Playwright;
//Type
using Bot.Types;
using Bot.Utils;
using System.Globalization;
using System.Text.Json;
using Bot.Service;
using Discord;
using System.Linq.Expressions;

namespace Bot.GerarImagemBrowser
{
    class GerarImagem()
    {
        //Path ate a pasta ondem vai fica as imagens.
        private static string PathPastaImagens = Path.Combine(AppContext.BaseDirectory, "imagens-gerador-info");
        //Broswer.
        public static IBrowser Browser;
        public static async Task Init()
        {
            var pw = await Playwright.CreateAsync();
            Browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = true
            });
        }   

        //Criar a pasta.
        private static async Task CriarPastaImagens()
        {
            if (!File.Exists(PathPastaImagens)) _ = Task.Run(() => Directory.CreateDirectory(PathPastaImagens));
        }


        //Gerar a imagem que mostrar as informacoes do usario
        public static async Task<string> SendMessagemShowInfo(UserInforCall dados)
        {
            //Criando a pasta caso ela nao exista
            await CriarPastaImagens();
            //Criando o contexto
            IBrowserContext context = await Browser.NewContextAsync();
            //nova pagina 
            IPage pagina = await context.NewPageAsync();
            //pegando o templete
            string path = @"file:///templete/showinfo.html";
            await pagina.GotoAsync(path);
            //elemento ondem nois vai tirar a print
            ILocator conteiner = pagina.Locator(".conteiner");
            //Antes vamos calcular a porcentagem
            long maxms = dados.Level * 100000;
            long ms = dados.Ms;
            //fazendoa a porcentagem e  limitado para 2 casas decimais
            double porcentagem = Math.Round((double)ms / maxms * 100, 2);
            //Mundando o valores do conteiner
            await pagina.EvaluateAsync(@"(dados) => {
                document.getElementById('nome').textContent = dados.Nome
                document.getElementById('level').textContent = dados.Level
                document.getElementById('tempo').textContent = dados.Tempo
                //Progresso
                document.querySelector('.text-progress').textContent = dados.pc
                document.querySelector('.progress').style.width = (dados.pc).replace(',', '.')
            }", new
            {
                Nome = dados.Nome,
                Level = dados.Level,
                pc = porcentagem + "%",
                Tempo = UtilsFN.FormatTime((ulong)dados.TotalMs)
            });
            //Tirando a print
            //ID
            string ID = Guid.NewGuid().ToString();
            //Path final seria o path da imagem
            string PathFinal = Path.Combine(PathPastaImagens, ID + ".png");
            await conteiner.ScreenshotAsync(new()
            {
                Path = PathFinal
            });

            await pagina.CloseAsync();
            //retornando o caminho da imagem.
            return PathFinal;
        }

        public static async Task<string> SendMessagemLevelUp(UserInforCall dados)
        {
            //Criando o novo contexto
            IBrowserContext context = await Browser.NewContextAsync();
            //Criando a pagina
            var pagina = await context.NewPageAsync();
            //Acessado o templete
            string path = "file:///templete/levelup.html";
            await pagina.GotoAsync(path);
            //Pegando o conteiner
            ILocator conteiner = pagina.Locator(".conteiner");
            //Modificando os valores
            //Level up
            string levelup = "Level " + dados.Level;
            //Nome
            string Nome = dados.Nome;
            //src/url do usuario
            IUser user = await BotServiceClient.Client.GetUserAsync(dados.ID); // pegando o usuario
            //url da imagem
            string src = user.GetAvatarUrl(ImageFormat.Auto, 4096);
            //add os valores
            await pagina.EvaluateAsync(@"(dados) => {
                document.getElementById('level').textContent = dados.Level
                document.getElementById('nome').textContent = dados.Nome
                document.getElementById('avatar').src = dados.Src
            }", new
            {
                Level = levelup,
                Nome = Nome,
                Src = src
            });
            //Printa e salvar
            string ID = Guid.NewGuid().ToString();
            string PathFinal = Path.Combine(PathPastaImagens, ID + ".png");
            await conteiner.ScreenshotAsync(new()
            {
                Path = PathFinal
            });
            return PathFinal;
        }
    }
} 