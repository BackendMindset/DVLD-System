using System;

namespace DVLD.DataAccessLayer
{
    public class RequestMedicalTestData
    {
        public int ID { get; set; } = -1;
        public int ApplicationID { get; set; } = -1;
        public int CenterID { get; set; } = -1;
        public int TestTypeID { get; set; } = -1;
        public string Status { get; set; } = "";
        public DateTime? ExamDate { get; set; } = null;
        public decimal PaidFees { get; set; } = 0m;
    }
}