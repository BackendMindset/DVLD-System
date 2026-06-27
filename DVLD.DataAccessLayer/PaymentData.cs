using System;

namespace DVLD.DataAccessLayer
{
    public class PaymentData
    {
        public int ID { get; set; } = -1;
        public int ApplicationID { get; set; } = -1;
        public decimal Amount { get; set; } = 0m;
        public string PaymentMethod { get; set; } = "";
        public string Status { get; set; } = "";
        public string ReferenceNumber { get; set; } = "";
        public DateTime PaymentDate { get; set; } = DateTime.MinValue;
    }
}