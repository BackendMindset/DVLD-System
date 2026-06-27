using System;

namespace DVLD.DataAccessLayer
{
    public class LicenseData
    {
        public int ID { get; set; } = -1;
        public int ApplicationID { get; set; } = -1;
        public int DriverID { get; set; } = -1;
        public int LicenseClassID { get; set; } = -1;
        public int CreatedByUserID { get; set; } = -1;
        public DateTime IssueDate { get; set; } = DateTime.MinValue;
        public DateTime ExpirationDate { get; set; } = DateTime.MinValue;
        public decimal PaidFees { get; set; } = 0m;
        public string Notes { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public byte IssueReason { get; set; } = 0;
        public string LicenseStatus { get; set; } = "";
    }

    public class LicenseListData
    {
        public int LicenseID { get; set; }
        public string DriverName { get; set; }
        public string ClassName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string LicenseStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
