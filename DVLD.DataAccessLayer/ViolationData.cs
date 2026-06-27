using System;

namespace DVLD.DataAccessLayer
{
    public class ViolationData
    {
        public int ID { get; set; } = -1;
        public int ViolationTypeID { get; set; } = -1;
        public int PersonID { get; set; } = -1;
        public int? LicenseID { get; set; } = null;
        public string Location { get; set; } = "";
        public DateTime IssueDate { get; set; } = DateTime.MinValue;
        public decimal FineAmount { get; set; } = 0m;
        public string Status { get; set; } = "";
        public string Notes { get; set; } = "";
    }

    public class ViolationListData
    {
        public int ViolationID { get; set; }
        public string ViolationType { get; set; }
        public string PersonName { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal FineAmount { get; set; }
        public string Status { get; set; }
    }
}
