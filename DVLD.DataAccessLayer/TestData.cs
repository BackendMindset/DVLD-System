using System;

namespace DVLD.DataAccessLayer
{
    public class TestData
    {
        public int ID { get; set; } = -1;
        public int TestAppointmentID { get; set; } = -1;
        public int CreatedByUserID { get; set; } = -1;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public byte TestResult { get; set; } = 0;
        public string Notes { get; set; } = "";
    }
}