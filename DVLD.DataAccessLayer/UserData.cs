using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccess
{
    public class UserData
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserData()
        {
            ID = -1;
            PersonID = -1;
            UserName = string.Empty;
            PasswordHash = string.Empty;
            IsActive = false;
            LastLogin = null;
        }
    }
}
