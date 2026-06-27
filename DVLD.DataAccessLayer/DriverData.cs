using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public class DriverData
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsFound { get; set; }

        public DriverData()
        {
            ID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.MinValue;
            IsFound = false;
        }
    }

    public class DriverListData
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public string NationalID { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
