//Type
using Bot.Types;

namespace Bot.GlobalVar
{
    class Global
    {
        public static Dictionary<ulong, UserInforCall> DicUserInforCall = new Dictionary<ulong, UserInforCall>()
        {
            {1021547876582690846, new()
            {
                Nome = "apolo300",
                Level = 1,
                MiliSegundos = 100000
            }
            }
        };
        public static Dictionary<ulong, UserInfoSw> DicUserSw = new Dictionary<ulong, UserInfoSw>();
    }
}