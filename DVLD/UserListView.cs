using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD
{
    public class UserListView
    {
        [DisplayName ("User ID")]
        public int UserID { get; set; }
        [DisplayName ("Person ID")]
        public int PersonID { get; set; }
        [DisplayName ("Full Name")]
        public string FullName { get; set; }
        [DisplayName ("User Name")]
        public string UserName { get; set; }
        [DisplayName ("Is Active")]
        public bool IsActive { get; set; }
    }
}
