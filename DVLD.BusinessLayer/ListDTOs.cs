using System;

namespace DVLD.BusinessLayer
{
    public class ApplicationListDto
    {
        public int ApplicationID { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicationType { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public decimal PaidFees { get; set; }
    }

    public class LicenseListDto
    {
        public int LicenseID { get; set; }
        public string DriverName { get; set; }
        public string ClassName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string LicenseStatus { get; set; }
        public bool IsActive { get; set; }
    }

    public class TestAppointmentListDto
    {
        public int TestAppointmentID { get; set; }
        public string TestTypeTitle { get; set; }
        public string ApplicantName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsLocked { get; set; }
    }

    public class ViolationListDto
    {
        public int ViolationID { get; set; }
        public string ViolationType { get; set; }
        public string PersonName { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal FineAmount { get; set; }
        public string Status { get; set; }
    }

    public class RoleListDto
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class PermissionListDto
    {
        public int PermissionID { get; set; }
        public string PermissionName { get; set; }
    }

    public class MedicalCenterListDto
    {
        public int CenterID { get; set; }
        public string CenterName { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentListDto
    {
        public int PaymentID { get; set; }
        public int ApplicationID { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class DriverListDto
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public string NationalID { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class LocalDrivingLicenseApplicationListDto
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public string Notes { get; set; }
    }

    public class DashboardStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalApplications { get; set; }
        public int NewApplications { get; set; }
        public int CompletedApplications { get; set; }
        public int CancelledApplications { get; set; }
        public int ActiveLicenses { get; set; }
        public int OpenDetainedLicenses { get; set; }
    }

    public class ApplicationStatisticsDto
    {
        public int TotalApplications { get; set; }
        public int NewApplications { get; set; }
        public int CompletedApplications { get; set; }
        public int CancelledApplications { get; set; }
    }

    public class LicenseStatisticsDto
    {
        public int TotalLicenses { get; set; }
        public int ActiveLicenses { get; set; }
        public int InactiveLicenses { get; set; }
        public int OpenDetainedLicenses { get; set; }
    }
}
