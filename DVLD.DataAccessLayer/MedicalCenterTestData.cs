using System;

namespace DVLD.DataAccessLayer
{
    public class MedicalCenterTestData
    {
        public int ID { get; set; } = -1;
        public int CenterID { get; set; } = -1;
        public int TestTypeID { get; set; } = -1;
        public decimal Fee { get; set; } = 0m;
    }
}