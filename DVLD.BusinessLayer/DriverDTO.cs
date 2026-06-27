using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public class DriverDto
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string FullName { get; set; }
        public string NationalID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
