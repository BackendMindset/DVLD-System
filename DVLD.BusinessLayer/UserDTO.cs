using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public class UserDto
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
