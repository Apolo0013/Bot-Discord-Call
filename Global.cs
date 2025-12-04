//Type
using Bot.Types;

namespace Bot.GlobalVar
{
    class Global
    {
        //Banco de dados
        public static Dictionary<ulong, UserInforCall> DicUserInforCall = new Dictionary<ulong, UserInforCall>();
        //Dicionario ondem guardar os Sw
        public static Dictionary<ulong, UserInfoSw> DicUserSw = new Dictionary<ulong, UserInfoSw>();
    }
}