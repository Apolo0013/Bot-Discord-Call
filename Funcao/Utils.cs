//global
using Bot.GlobalVar;
//Type
using Bot.Types;
//Outros
using System.Text;
using System.Text.Json;

namespace Bot.Utils
{
    class UtilsFN
    {
        //Path ate o json ondem guardar as informacoes
        private static string PathCallJson = Path.Combine(AppContext.BaseDirectory, "dados", "callinfo.json");


        public static string FormatTime(ulong ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            //string final
            string msgfinal = "";
            //Horas, Minutos e Segundos
            int horas = t.Hours;
            int minutos = t.Minutes;
            int segundos = t.Seconds;
            //Tratamentos...
            if (horas > 0) msgfinal += (horas <= 9 ? "0" + horas.ToString() : horas) + ":";
            if (minutos > 0 || horas > 0) msgfinal += (minutos <= 9 ? "0" + minutos.ToString() : minutos) + ":";
            if (segundos > 0 || minutos > 0 || horas > 0) msgfinal += segundos <= 9 ? "0" + segundos.ToString() : segundos;
            //Caso especial, se somente o segundos estive sozin pae
            if (minutos == 0 && horas == 0) msgfinal += "s";
            //retornando a string ja tratada
            return msgfinal;
        }

        //Funcao que vai ler e escrever no Global "DicUserInforCall"
        public static async Task<Dictionary<ulong, UserInforCall>>? Lercallinfojson()
        {
            //lendo
            string jsonstring = await File.ReadAllTextAsync(PathCallJson, Encoding.UTF8);
            //deserializando
            Dictionary<ulong, UserInforCall>? dados = JsonSerializer.Deserialize<Dictionary<ulong, UserInforCall>>(jsonstring);
            if (dados == null) return null!;
            return dados;
        }

        //essa funcao vai escrever o DicUserInforCall atual no json
        public static async Task Escrevercallinfojson()
        {
            //transoformando em string
            string jsonstring = JsonSerializer.Serialize(Global.DicUserInforCall, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });
            //Escrevendo...
            await File.WriteAllTextAsync(PathCallJson, jsonstring);
        }
    }
}