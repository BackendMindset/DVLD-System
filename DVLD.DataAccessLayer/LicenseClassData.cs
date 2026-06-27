using System;

namespace DVLD.DataAccessLayer
{
    public class LicenseClassData
    {
        public int ID { get; set; } = -1;
        public string ClassName { get; set; } = "";
        public string ClassDescription { get; set; } = "";
        public byte MinimumAllowedAge { get; set; } = 0;
        public byte ValidityLength { get; set; } = 0;
        public decimal LicenseFee { get; set; } = 0m;
    }
}