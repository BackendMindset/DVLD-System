using System;

namespace DVLD.DataAccessLayer
{
    public class PaymentDetailData
    {
        public int ID { get; set; } = -1;
        public int PaymentID { get; set; } = -1;
        public decimal Amount { get; set; } = 0m;
        public string ItemName { get; set; } = "";
    }
}