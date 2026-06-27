using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public class UserListDto
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}
