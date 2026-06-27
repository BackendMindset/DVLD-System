using System;

namespace DVLD.DataAccessLayer
{
    public class LocalDrivingLicenseApplicationData
    {
        public int ID { get; set; } = -1;
        public int ApplicationID { get; set; } = -1;
        public int LicenseClassID { get; set; } = -1;
        public string Notes { get; set; } = "";
    }

    public class LocalDrivingLicenseApplicationListData
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public string Notes { get; set; } = "";
    }
}
