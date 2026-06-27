using System;

namespace DVLD.DataAccessLayer
{
    public class MedicalTestResultData
    {
        public int ID { get; set; } = -1;
        public int RequestMedicalTestID { get; set; } = -1;
        public bool Result { get; set; } = false;
        public string ResultDetails { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public int? CreatedByUserID { get; set; } = null;
    }
}