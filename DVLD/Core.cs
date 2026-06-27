using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DVLD
{
    public static class Session
    {
        public static UserService CurrentUser { get; private set; }

        public static void Start(UserService user)
        {
            CurrentUser = user;
        }
        public static void End()
        {
            CurrentUser = null;
        }
        public static bool IsLoggedIn => CurrentUser != null;
    }
}
