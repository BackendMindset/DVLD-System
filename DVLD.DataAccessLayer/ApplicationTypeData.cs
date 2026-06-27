using System;

namespace DVLD.DataAccessLayer
{
    public class ApplicationTypeData
    {
        public int ID { get; set; } = -1;
        public string ApplicationTypeTitle { get; set; } = "";
        public decimal ApplicationFees { get; set; } = 0m;
    }
}