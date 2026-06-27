using System;

namespace DVLD.DataAccessLayer
{
    public class ViolationTypeData
    {
        public int ID { get; set; } = -1;
        public string ViolationName { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal BaseFineAmount { get; set; } = 0m;
    }
}