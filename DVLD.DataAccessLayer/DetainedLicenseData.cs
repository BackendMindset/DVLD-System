using System;

namespace DVLD.DataAccessLayer
{
    public class DetainedLicenseData
    {
        public int ID { get; set; } = -1;
        public int LicenseID { get; set; } = -1;
        public decimal FineFees { get; set; } = 0m;
        public DateTime DetainDate { get; set; } = DateTime.MinValue;
        public bool IsReleased { get; set; } = false;
        public DateTime? ReleaseDate { get; set; } = null;
        public int? ReleaseApplicationID { get; set; } = null;
        public int? ReleasedByUserID { get; set; } = null;
    }
}