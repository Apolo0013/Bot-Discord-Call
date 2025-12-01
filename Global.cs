//Type
using System.Diagnostics;
using Bot.Types;

namespace Bot.GlobalVar
{
    class Global
    {
        public static Dictionary<ulong, UserInforCall> DicUserInforCall = new Dictionary<ulong, UserInforCall>();
        public static Dictionary<ulong, UserInfoSw> DicUserSw = new Dictionary<ulong, UserInfoSw>();
    }
}