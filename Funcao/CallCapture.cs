//Class que vai manipular dados de acordo entrar e sair de call.
using System.Net.Sockets;
using Discord.WebSocket;
//Global
using Bot.GlobalVar;
//Utils Discord
using Bot.Funcao.Utils_Discord;
//Type
using Bot.Types;
using System.Diagnostics;
using System.Text.Json;
using Discord;

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
            //Criando a chave caso nao exista
            if (!Global.DicUserInforCall.ContainsKey(userid))
            {
                Global.DicUserInforCall.Add(userid, new UserInforCall()
                {
                    Nome = user.GlobalName,
                    MiliSegundos = 0
                });
            }
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
            long Miliseg = Global.DicUserSw[userid].Sw.ElapsedMilliseconds;
            //Colocando os milisegundos no Dicionario Global
            Global.DicUserInforCall[userid].MiliSegundos += Miliseg; // Somando...
            //Reserta a contagem    
            Global.DicUserSw[userid].Sw.Restart();
            //...
            //Mandando uma messagem para o usuario avisando o usuario quanto tempo ele estava na call.
            Console.WriteLine(Miliseg);
            string nomecall = Global.DicUserSw[userid].UltimaCall; // a ultima call que o cara esteve ou seja oq ele acabou de sair.
            string messagem = $"Voce passou {FormatTime((ulong)Miliseg)}, na call **{nomecall}**";
            await UtilsDiscord.SendMessagemDM(userid, messagem);
        }

        //Mostrar o tempo.
        public static string ShowTime(ulong userid)
        {
            //Estou somando a contagem que esta sendo contado com a quantidade ded mili dentro do DicGLobal ondem fica o dados pesistente do user. Assim para aparece realmente que esta contando. Mas so conta quando ele sair da call memo o.

            ulong ms = (ulong)(Global.DicUserInforCall[userid].MiliSegundos + Global.DicUserSw[userid].Sw.ElapsedMilliseconds);
            return FormatTime(ms);
        }
        
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
    }
}