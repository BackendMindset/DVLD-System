using System;

namespace DVLD.DataAccessLayer
{
    public class TestTypeData
    {
        public int ID { get; set; } = -1;
        public string TestTypeTitle { get; set; } = "";
        public string TestTypeDescription { get; set; } = "";
        public decimal TestTypeFees { get; set; } = 0m;
        public bool IsRequired { get; set; } = false;
    }
}