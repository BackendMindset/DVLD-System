using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class LicenseService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal PaidFees { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public byte IssueReason { get; set; }
        public string LicenseStatus { get; set; }

        private LicenseService()
        {
            ID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClassID = -1;
            CreatedByUserID = -1;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            PaidFees = 0m;
            Notes = string.Empty;
            IsActive = false;
            IssueReason = 0;
            LicenseStatus = string.Empty;
            Mode = enMode.AddNew;
        }

        private LicenseService(LicenseData data)
        {
            ID = data.ID;
            ApplicationID = data.ApplicationID;
            DriverID = data.DriverID;
            LicenseClassID = data.LicenseClassID;
            CreatedByUserID = data.CreatedByUserID;
            IssueDate = data.IssueDate;
            ExpirationDate = data.ExpirationDate;
            PaidFees = data.PaidFees;
            Notes = data.Notes ?? string.Empty;
            IsActive = data.IsActive;
            IssueReason = data.IssueReason;
            LicenseStatus = data.LicenseStatus ?? string.Empty;
            Mode = enMode.Update;
        }

        private LicenseData MapToData()
        {
            return new LicenseData
            {
                ID = ID,
                ApplicationID = ApplicationID,
                DriverID = DriverID,
                LicenseClassID = LicenseClassID,
                CreatedByUserID = CreatedByUserID,
                IssueDate = IssueDate,
                ExpirationDate = ExpirationDate,
                PaidFees = PaidFees,
                Notes = Notes?.Trim(),
                IsActive = IsActive,
                IssueReason = IssueReason,
                LicenseStatus = LicenseStatus?.Trim(),
            };
        }

        private Result<bool> Validate()
        {
            if (ApplicationID <= 0) return Result<bool>.Fail("Valid application ID is required.");
            if (DriverID <= 0) return Result<bool>.Fail("Valid driver ID is required.");
            if (LicenseClassID <= 0) return Result<bool>.Fail("Valid license class ID is required.");
            if (ExpirationDate <= IssueDate) return Result<bool>.Fail("Expiration date must be after issue date.");
            if (PaidFees < 0) return Result<bool>.Fail("Paid fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<LicenseService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await LicensesDataAccess.GetByIDAsync(id);
            return data != null ? new LicenseService(data) : null;
        }

        public static async Task<LicenseService> FindByDriverIDAsync(int driverid)
        {
            if (driverid <= 0) return null;
            var list = await LicensesDataAccess.GetByDriverIdAsync(driverid);
            var data = list.FirstOrDefault(x => x.DriverID == driverid);
            return data != null ? new LicenseService(data) : null;
        }

        public static async Task<List<LicenseService>> GetAllAsync()
        {
            var dataList = await LicensesDataAccess.GetAllAsync();
            var list = new List<LicenseService>();
            foreach (var d in dataList)
                list.Add(new LicenseService(d));
            return list;
        }

        public async Task<Result<bool>> ValidateLicenseAsync()
        {
            Result<bool> validation = Validate();
            if (!validation.Success)
                return validation;
            if (!await ApplicationsDataAccess.ExistsAsync(ApplicationID))
                return Result<bool>.Fail("Application does not exist.");
            DriverData driver = await DriversDataAccess.GetByIDAsync(DriverID);
            if (driver == null || !driver.IsFound)
                return Result<bool>.Fail("Driver does not exist.");
            return Result<bool>.Ok(true);
        }

        public static async Task<Result<bool>> CanIssueLicenseAsync(int applicationId, int driverId, int licenseClassId)
        {
            if (applicationId <= 0 || driverId <= 0 || licenseClassId <= 0)
                return Result<bool>.Fail("Application, driver, and license class are required.");
            ApplicationService application = await ApplicationService.FindByIDAsync(applicationId);
            if (application == null)
                return Result<bool>.Fail("Application does not exist.");
            if (application.ApplicationStatus != ApplicationService.NewStatus)
                return Result<bool>.Fail("License can only be issued for a new application.");
            LicenseService existingLicense = await FindByDriverIDAsync(driverId);
            if (existingLicense != null && existingLicense.LicenseClassID == licenseClassId && existingLicense.IsActive)
                return Result<bool>.Fail("Driver already has an active license for this class.");
            return Result<bool>.Ok(true, "License can be issued.");
        }

        public static async Task<List<LicenseService>> GetLicensesByDriverIdAsync(int driverId)
        {
            List<LicenseData> dataList = await LicensesDataAccess.GetByDriverIdAsync(driverId);
            return dataList.Select(x => new LicenseService(x)).ToList();
        }

        public static async Task<List<LicenseListDto>> SearchLicensesAsync(string value)
        {
            List<LicenseListData> dataList = await LicensesDataAccess.SearchLicensesAsync(value);
            return dataList.Select(x => new LicenseListDto
            {
                LicenseID = x.LicenseID,
                DriverName = x.DriverName,
                ClassName = x.ClassName,
                IssueDate = x.IssueDate,
                ExpirationDate = x.ExpirationDate,
                LicenseStatus = x.LicenseStatus,
                IsActive = x.IsActive
            }).ToList();
        }

        public static async Task<List<LicenseListDto>> GetLicensesForListAsync()
        {
            List<LicenseListData> dataList = await LicensesDataAccess.GetLicensesForListAsync();
            return dataList.Select(x => new LicenseListDto
            {
                LicenseID = x.LicenseID,
                DriverName = x.DriverName,
                ClassName = x.ClassName,
                IssueDate = x.IssueDate,
                ExpirationDate = x.ExpirationDate,
                LicenseStatus = x.LicenseStatus,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = await ValidateLicenseAsync();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await LicensesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await LicensesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public async Task<Result<bool>> SuspendLicenseAsync(string reason = "Suspended")
        {
            if (!IsActive)
                return Result<bool>.Fail("License is already inactive.");

            IsActive = false;
            LicenseStatus = string.IsNullOrWhiteSpace(reason) ? "Suspended" : reason.Trim();
            return await SaveAsync();
        }

        public async Task<Result<bool>> RestoreLicenseAsync()
        {
            if (IsActive)
                return Result<bool>.Fail("License is already active.");

            IsActive = true;
            LicenseStatus = "Active";
            return await SaveAsync();
        }

        public static async Task<LicenseStatisticsDto> GetLicenseStatisticsAsync()
        {
            int total = (await GetAllAsync()).Count;
            int active = await LicensesDataAccess.GetActiveLicensesCountAsync();
            int detained = await DetainedLicensesDataAccess.GetOpenDetainedLicensesCountAsync();

            return new LicenseStatisticsDto
            {
                TotalLicenses = total,
                ActiveLicenses = active,
                InactiveLicenses = total - active,
                OpenDetainedLicenses = detained
            };
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await LicensesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
