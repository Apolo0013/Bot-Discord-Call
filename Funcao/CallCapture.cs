//Class que vai manipular dados de acordo entrar e sair de call.
using System.Net.Sockets;
using Discord.WebSocket;
//Global
using Bot.GlobalVar;
//Utils Discord
using Bot.Funcao.Utils_Discord;
using Bot.Utils;
//Type
using Bot.Types;
using System.Diagnostics;


namespace Bot.Funcao.CallCapture
{
    //Class que vai receber
    //Entradas em Call
    //Sair em call
    class CallCapture()
    {
        public static async Task EntrarCall(SocketUser user, string nomecall)
        {
            //ID do usuario
            ulong userid = user.Id;
            await UtilsDiscord.VerificarRegistror(user);
            //Craindo a chave no dicionario Global ondem fica o Stopwatch
            if (!Global.DicUserSw.ContainsKey(userid))
            {
                Global.DicUserSw.Add(userid, new UserInfoSw()
                {
                    Sw = new Stopwatch(),
                    UltimaCall = nomecall
                });
            }
            //Depois de passa por todas essa verificacao de chave, vamos trabalha com elas
            //Iniciando a contagem
            Global.DicUserSw[userid].Sw.Start(); // inciando a contagem.
            //Agora ele esta contando...
        }

        //Sera chamado quando o usuario sair da call.
        public static async Task SairCall(ulong userid)
        {
            //antes vamos verificar:
            //ele existir na lista?
            if (!Global.DicUserInforCall.ContainsKey(userid)) return;
            //Porque nao vamos verificar as chaves?
            //resposta: porque antes de sair ele entrou, quando ele entrar, ja tinha fazido* isso.
            //Seguinte...
            // - Vamos parar de conta.
            // - Pegar o segundos em call
            // - Colocar ele no Dicionario global
            // - Reserta a contagem dele(no DicUserSw).
            //Parar de Conta
            Global.DicUserSw[userid].Sw.Stop();
            //Peger os segundos em call
            long Ms = Global.DicUserSw[userid].Sw.ElapsedMilliseconds;
            //========================================================
            //Como os ms na call vamos somar
            // - somar no TotalMs (ondem vai fica o total de tempo de todas as call)
            // - somar no Ms (ondem ele aguardar o tempo de cada nivel)
            // Ex: level1 = 100000 ele vai guardar ate 100k e depois zera.
            // level2 = 200000 ele vai guardar ate 200k e dedpois zera.
            Global.DicUserInforCall[userid].TotalMs += Ms; // Somando no total
            Global.DicUserInforCall[userid].Ms += Ms;
            //Reserta a contagem    
            Global.DicUserSw[userid].Sw.Reset();
            //...
            //Mandando uma messagem para o usuario avisando o usuario quanto tempo ele estava na call.
            string nomecall = Global.DicUserSw[userid].UltimaCall; // a ultima call que o cara esteve ou seja oq ele acabou de sair.
            string messagem = $"Voce passou {UtilsFN.FormatTime((ulong)Ms)}, na call **{nomecall}**";
            await UtilsDiscord.SendMessagemDM(userid, messagem);
        }

        //Mostrar o tempo.
        public static string ShowTime(ulong userid)
        {
            //Estou somando a contagem que esta sendo contado com a quantidade ded mili dentro do DicGLobal ondem fica o dados pesistente do user. Assim para aparece realmente que esta contando. Mas so conta quando ele sair da call memo o.
            ulong ms = (ulong)(Global.DicUserInforCall[userid].TotalMs + Global.DicUserSw[userid].Sw.ElapsedMilliseconds);
            return UtilsFN.FormatTime(ms);
        }
    }
}