using System.Diagnostics;

namespace Bot.Types
{
    class UserInforCall
    {
        //Id ja tem na chave, mas as vezes nao temos acesso ao memso
        public ulong  ID { set; get; }
        public string NomeGlobal { set; get; } = "";
        public string Nome { set; get; } = "";
        public long Ms { set; get; }
        public long TotalMs { set; get; }
        public int Level { set; get; }
    }

    class UserInfoSw
    {
        public Stopwatch Sw { set; get; } = new Stopwatch();
        public string UltimaCall { set; get; } = "";
    }
}