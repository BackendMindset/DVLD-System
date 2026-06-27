using System;

namespace DVLD.DataAccessLayer
{
    public class TestAppointmentData
    {
        public int ID { get; set; } = -1;
        public int LocalDrivingLicenseApplicationID { get; set; } = -1;
        public int TestTypeID { get; set; } = -1;
        public int CreatedByUserID { get; set; } = -1;
        public DateTime AppointmentDate { get; set; } = DateTime.MinValue;
        public decimal PaidFees { get; set; } = 0m;
        public bool IsLocked { get; set; } = false;
    }
}