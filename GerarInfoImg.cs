using Bot.Funcao.levelUp;
using Microsoft.Playwright;
//Type
using Bot.Types;
using Bot.Utils;
using System.Globalization;
using System.Text.Json;

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

        //Gerar a imagem
        public static async Task<string> GerarImagemFN(UserInforCall dados)
        {
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
            long ms = dados.MiliSegundos;
            double porcentagem = (double)ms / maxms * 100;
            Console.WriteLine(ms / maxms );
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                Nome = dados.Nome,
                Level = dados.Level,
                pc = porcentagem + "%",
                Tempo = UtilsFN.FormatTime((ulong)dados.MiliSegundos)
            }
            ));
            //Mundando o valores do conteiner
            await pagina.EvaluateAsync(@"(dados) => {
                document.getElementById('nome').textContent = dados.Nome
                document.getElementById('level').textContent = dados.Level
                document.getElementById('tempo').textContent = dados.Tempo
                //Progresso
                document.querySelector('.text-progress').textContent = dados.pc
                document.querySelector('.progress').style.width = dados.pc
            }", new
            {
                Nome = dados.Nome,
                Level = dados.Level,
                pc = porcentagem + "%",
                Tempo = UtilsFN.FormatTime((ulong)dados.MiliSegundos)
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
            //retornando o caminho da imagem.
            return PathFinal;
        }
    }
} 