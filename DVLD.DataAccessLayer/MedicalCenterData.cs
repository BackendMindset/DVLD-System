using System;

namespace DVLD.DataAccessLayer
{
    public class MedicalCenterData
    {
        public int ID { get; set; } = -1;
        public string CenterName { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool IsActive { get; set; } = false;
    }
}