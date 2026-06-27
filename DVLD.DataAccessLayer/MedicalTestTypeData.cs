using System;

namespace DVLD.DataAccessLayer
{
    public class MedicalTestTypeData
    {
        public int ID { get; set; } = -1;
        public string TestName { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal DefaultFee { get; set; } = 0m;
        public bool IsRequired { get; set; } = false;
    }
}