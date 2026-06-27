using System;

namespace DVLD.DataAccessLayer
{
    public class ApplicationData
    {
        public int ID { get; set; } = -1;
        public int ApplicantPersonID { get; set; } = -1;
        public int ApplicationTypeID { get; set; } = -1;
        public int CreatedByUserID { get; set; } = -1;
        public DateTime ApplicationDate { get; set; } = DateTime.MinValue;
        public byte ApplicationStatus { get; set; } = 0;
        public DateTime LastStatusDate { get; set; } = DateTime.MinValue;
        public decimal PaidFees { get; set; } = 0m;
        public DateTime? UpdatedAt { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public bool IsActive { get; set; } = false;
    }

    public class ApplicationListData
    {
        public int ApplicationID { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicationType { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public decimal PaidFees { get; set; }
    }
}
