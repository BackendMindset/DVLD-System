using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class ApplicationService
    {
        public const byte NewStatus = 1;
        public const byte CancelledStatus = 2;
        public const byte CompletedStatus = 3;

        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ApplicantPersonID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        private ApplicationService()
        {
            ID = -1;
            ApplicantPersonID = -1;
            ApplicationTypeID = -1;
            CreatedByUserID = -1;
            ApplicationDate = DateTime.MinValue;
            ApplicationStatus = 0;
            LastStatusDate = DateTime.MinValue;
            PaidFees = 0m;
            UpdatedAt = null;
            CreatedAt = DateTime.MinValue;
            IsActive = false;
            Mode = enMode.AddNew;
        }

        private ApplicationService(ApplicationData data)
        {
            ID = data.ID;
            ApplicantPersonID = data.ApplicantPersonID;
            ApplicationTypeID = data.ApplicationTypeID;
            CreatedByUserID = data.CreatedByUserID;
            ApplicationDate = data.ApplicationDate;
            ApplicationStatus = data.ApplicationStatus;
            LastStatusDate = data.LastStatusDate;
            PaidFees = data.PaidFees;
            UpdatedAt = data.UpdatedAt;
            CreatedAt = data.CreatedAt;
            IsActive = data.IsActive;
            Mode = enMode.Update;
        }

        private ApplicationData MapToData()
        {
            return new ApplicationData
            {
                ID = ID,
                ApplicantPersonID = ApplicantPersonID,
                ApplicationTypeID = ApplicationTypeID,
                CreatedByUserID = CreatedByUserID,
                ApplicationDate = ApplicationDate,
                ApplicationStatus = ApplicationStatus,
                LastStatusDate = LastStatusDate,
                PaidFees = PaidFees,
                UpdatedAt = UpdatedAt,
                CreatedAt = CreatedAt,
                IsActive = IsActive,
            };
        }

        private Result<bool> Validate()
        {
            if (ApplicantPersonID <= 0) return Result<bool>.Fail("Valid applicant person ID is required.");
            if (ApplicationTypeID <= 0) return Result<bool>.Fail("Valid application type ID is required.");
            if (CreatedByUserID <= 0) return Result<bool>.Fail("Valid creator user ID is required.");
            if (PaidFees < 0) return Result<bool>.Fail("Paid fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<ApplicationService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await ApplicationsDataAccess.GetByIDAsync(id);
            return data != null ? new ApplicationService(data) : null;
        }

        public static async Task<ApplicationService> FindByPersonIDAsync(int applicantpersonid)
        {
            if (applicantpersonid <= 0) return null;
            var list = await ApplicationsDataAccess.GetByPersonIdAsync(applicantpersonid);
            var data = list.FirstOrDefault(x => x.ApplicantPersonID == applicantpersonid);
            return data != null ? new ApplicationService(data) : null;
        }

        public static async Task<List<ApplicationService>> GetAllAsync()
        {
            var dataList = await ApplicationsDataAccess.GetAllAsync();
            var list = new List<ApplicationService>();
            foreach (var d in dataList)
                list.Add(new ApplicationService(d));
            return list;
        }

        public static async Task<List<ApplicationService>> GetApplicationsByPersonIdAsync(int personId)
        {
            List<ApplicationData> dataList = await ApplicationsDataAccess.GetByPersonIdAsync(personId);
            return dataList.Select(x => new ApplicationService(x)).ToList();
        }

        public static async Task<List<ApplicationListDto>> SearchApplicationsAsync(string value)
        {
            List<ApplicationListData> dataList = await ApplicationsDataAccess.SearchApplicationsAsync(value);
            return dataList.Select(x => new ApplicationListDto
            {
                ApplicationID = x.ApplicationID,
                ApplicantName = x.ApplicantName,
                ApplicationType = x.ApplicationType,
                ApplicationDate = x.ApplicationDate,
                Status = x.Status,
                PaidFees = x.PaidFees
            }).ToList();
        }

        public static async Task<List<ApplicationListDto>> GetApplicationsForListAsync()
        {
            List<ApplicationListData> dataList = await ApplicationsDataAccess.GetApplicationsForListAsync();
            return dataList.Select(x => new ApplicationListDto
            {
                ApplicationID = x.ApplicationID,
                ApplicantName = x.ApplicantName,
                ApplicationType = x.ApplicationType,
                ApplicationDate = x.ApplicationDate,
                Status = x.Status,
                PaidFees = x.PaidFees
            }).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await ApplicationsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await ApplicationsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public async Task<Result<bool>> CancelApplicationAsync()
        {
            if (ApplicationStatus != NewStatus)
                return Result<bool>.Fail("Only new applications can be cancelled.");

            ApplicationStatus = CancelledStatus;
            LastStatusDate = DateTime.Now;
            return await SaveAsync();
        }

        public async Task<Result<bool>> CompleteApplicationAsync()
        {
            if (ApplicationStatus != NewStatus)
                return Result<bool>.Fail("Only new applications can be completed.");

            ApplicationStatus = CompletedStatus;
            LastStatusDate = DateTime.Now;
            return await SaveAsync();
        }

        public Task<Result<bool>> ApproveApplicationAsync()
        {
            return Task.FromResult(Result<bool>.Fail("Current database schema supports only New, Cancelled, and Completed application statuses."));
        }

        public Task<Result<bool>> RejectApplicationAsync()
        {
            return Task.FromResult(Result<bool>.Fail("Current database schema supports only New, Cancelled, and Completed application statuses."));
        }

        public static async Task<ApplicationStatisticsDto> GetApplicationStatisticsAsync()
        {
            int total = await ApplicationsDataAccess.GetApplicationsCountAsync();
            int newApplications = await ApplicationsDataAccess.GetApplicationsCountByStatusAsync(NewStatus);
            int cancelledApplications = await ApplicationsDataAccess.GetApplicationsCountByStatusAsync(CancelledStatus);
            int completedApplications = await ApplicationsDataAccess.GetApplicationsCountByStatusAsync(CompletedStatus);

            return new ApplicationStatisticsDto
            {
                TotalApplications = total,
                NewApplications = newApplications,
                CancelledApplications = cancelledApplications,
                CompletedApplications = completedApplications
            };
        }

        public static async Task<DashboardStatisticsDto> GetDashboardStatisticsAsync()
        {
            ApplicationStatisticsDto applicationStats = await GetApplicationStatisticsAsync();
            LicenseStatisticsDto licenseStats = await LicenseService.GetLicenseStatisticsAsync();
            List<UserListDto> users = await UserService.GetUsersForListAsync();

            return new DashboardStatisticsDto
            {
                TotalUsers = users.Count,
                ActiveUsers = users.Count(x => x.IsActive),
                TotalApplications = applicationStats.TotalApplications,
                NewApplications = applicationStats.NewApplications,
                CompletedApplications = applicationStats.CompletedApplications,
                CancelledApplications = applicationStats.CancelledApplications,
                ActiveLicenses = licenseStats.ActiveLicenses,
                OpenDetainedLicenses = licenseStats.OpenDetainedLicenses
            };
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await ApplicationsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
