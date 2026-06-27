using System;

namespace DVLD.DataAccessLayer
{
    public class InternationalLicenseData
    {
        public int ID { get; set; } = -1;
        public int ApplicationID { get; set; } = -1;
        public int DriverID { get; set; } = -1;
        public int IssuedUsingLocalLicenseID { get; set; } = -1;
        public int CreatedByUserID { get; set; } = -1;
        public int IssuingCountryID { get; set; } = -1;
        public DateTime IssueDate { get; set; } = DateTime.MinValue;
        public DateTime ExpirationDate { get; set; } = DateTime.MinValue;
        public decimal PaidFees { get; set; } = 0m;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public bool IsActive { get; set; } = false;
    }
}