using System.Diagnostics;

namespace Bot.Types
{
    class UserInforCall
    {
        public string Nome { set; get; } = "";
        public long MiliSegundos { set; get; }
        public int Level { set; get; }
    }

    class UserInfoSw
    {
        public Stopwatch Sw { set; get; } = new Stopwatch();
        public string UltimaCall { set; get; } = "";
    }
}