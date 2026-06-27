using System;

namespace DVLD.DataAccessLayer
{
    public class ViolationPaymentData
    {
        public int ID { get; set; } = -1;
        public int ViolationID { get; set; } = -1;
        public int PaymentID { get; set; } = -1;
        public decimal PaidAmount { get; set; } = 0m;
        public DateTime PaidDate { get; set; } = DateTime.MinValue;
        public string Status { get; set; } = "";
    }
}