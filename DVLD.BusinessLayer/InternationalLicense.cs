using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class InternationalLicenseService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public int CreatedByUserID { get; set; }
        public int IssuingCountryID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal PaidFees { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        private InternationalLicenseService()
        {
            ID = -1;
            ApplicationID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;
            CreatedByUserID = -1;
            IssuingCountryID = -1;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            PaidFees = 0m;
            CreatedAt = DateTime.MinValue;
            IsActive = false;
            Mode = enMode.AddNew;
        }

        private InternationalLicenseService(InternationalLicenseData data)
        {
            ID = data.ID;
            ApplicationID = data.ApplicationID;
            DriverID = data.DriverID;
            IssuedUsingLocalLicenseID = data.IssuedUsingLocalLicenseID;
            CreatedByUserID = data.CreatedByUserID;
            IssuingCountryID = data.IssuingCountryID;
            IssueDate = data.IssueDate;
            ExpirationDate = data.ExpirationDate;
            PaidFees = data.PaidFees;
            CreatedAt = data.CreatedAt;
            IsActive = data.IsActive;
            Mode = enMode.Update;
        }

        private InternationalLicenseData MapToData()
        {
            return new InternationalLicenseData
            {
                ID = ID,
                ApplicationID = ApplicationID,
                DriverID = DriverID,
                IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID,
                CreatedByUserID = CreatedByUserID,
                IssuingCountryID = IssuingCountryID,
                IssueDate = IssueDate,
                ExpirationDate = ExpirationDate,
                PaidFees = PaidFees,
                CreatedAt = CreatedAt,
                IsActive = IsActive,
            };
        }

        private Result<bool> Validate()
        {
            if (ApplicationID <= 0) return Result<bool>.Fail("Valid application ID is required.");
            if (DriverID <= 0) return Result<bool>.Fail("Valid driver ID is required.");
            if (IssuedUsingLocalLicenseID <= 0) return Result<bool>.Fail("Valid local license ID is required.");
            if (ExpirationDate <= IssueDate) return Result<bool>.Fail("Expiration date must be after issue date.");
            if (PaidFees < 0) return Result<bool>.Fail("Paid fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<InternationalLicenseService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await InternationalLicensesDataAccess.GetByIDAsync(id);
            return data != null ? new InternationalLicenseService(data) : null;
        }

        public static async Task<InternationalLicenseService> FindByDriverIDAsync(int driverid)
        {
            if (driverid <= 0) return null;
            var list = await InternationalLicensesDataAccess.GetByDriverIdAsync(driverid);
            var data = list.FirstOrDefault(x => x.DriverID == driverid);
            return data != null ? new InternationalLicenseService(data) : null;
        }

        public static async Task<List<InternationalLicenseService>> GetAllAsync()
        {
            var dataList = await InternationalLicensesDataAccess.GetAllAsync();
            var list = new List<InternationalLicenseService>();
            foreach (var d in dataList)
                list.Add(new InternationalLicenseService(d));
            return list;
        }

        public static async Task<Result<bool>> CanIssueInternationalLicenseAsync(int driverId, int localLicenseId)
        {
            if (driverId <= 0 || localLicenseId <= 0)
                return Result<bool>.Fail("Driver and local license are required.");

            LicenseService localLicense = await LicenseService.FindByIDAsync(localLicenseId);
            if (localLicense == null)
                return Result<bool>.Fail("Local license does not exist.");
            if (localLicense.DriverID != driverId)
                return Result<bool>.Fail("Local license does not belong to the selected driver.");
            if (!localLicense.IsActive)
                return Result<bool>.Fail("International license requires an active local license.");
            if (await InternationalLicensesDataAccess.HasActiveLicenseForDriverAsync(driverId))
                return Result<bool>.Fail("Driver already has an active international license.");

            return Result<bool>.Ok(true, "International license can be issued.");
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await InternationalLicensesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await InternationalLicensesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await InternationalLicensesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
