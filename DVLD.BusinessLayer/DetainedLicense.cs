using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class DetainedLicenseService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int LicenseID { get; set; }
        public decimal FineFees { get; set; }
        public DateTime DetainDate { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleaseApplicationID { get; set; }
        public int? ReleasedByUserID { get; set; }

        private DetainedLicenseService()
        {
            ID = -1;
            LicenseID = -1;
            FineFees = 0m;
            DetainDate = DateTime.MinValue;
            IsReleased = false;
            ReleaseDate = null;
            ReleaseApplicationID = null;
            ReleasedByUserID = null;
            Mode = enMode.AddNew;
        }

        private DetainedLicenseService(DetainedLicenseData data)
        {
            ID = data.ID;
            LicenseID = data.LicenseID;
            FineFees = data.FineFees;
            DetainDate = data.DetainDate;
            IsReleased = data.IsReleased;
            ReleaseDate = data.ReleaseDate;
            ReleaseApplicationID = data.ReleaseApplicationID;
            ReleasedByUserID = data.ReleasedByUserID;
            Mode = enMode.Update;
        }

        private DetainedLicenseData MapToData()
        {
            return new DetainedLicenseData
            {
                ID = ID,
                LicenseID = LicenseID,
                FineFees = FineFees,
                DetainDate = DetainDate,
                IsReleased = IsReleased,
                ReleaseDate = ReleaseDate,
                ReleaseApplicationID = ReleaseApplicationID,
                ReleasedByUserID = ReleasedByUserID,
            };
        }

        private Result<bool> Validate()
        {
            if (LicenseID <= 0) return Result<bool>.Fail("Valid license ID is required.");
            if (FineFees < 0) return Result<bool>.Fail("Fine fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<DetainedLicenseService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await DetainedLicensesDataAccess.GetByIDAsync(id);
            return data != null ? new DetainedLicenseService(data) : null;
        }

        public static async Task<DetainedLicenseService> FindByLicenseIDAsync(int licenseid)
        {
            if (licenseid <= 0) return null;
            var data = await DetainedLicensesDataAccess.GetOpenByLicenseIdAsync(licenseid);
            return data != null ? new DetainedLicenseService(data) : null;
        }

        public static async Task<List<DetainedLicenseService>> GetAllAsync()
        {
            var dataList = await DetainedLicensesDataAccess.GetAllAsync();
            var list = new List<DetainedLicenseService>();
            foreach (var d in dataList)
                list.Add(new DetainedLicenseService(d));
            return list;
        }

        public static async Task<Result<bool>> CanDetainLicenseAsync(int licenseId)
        {
            if (licenseId <= 0)
                return Result<bool>.Fail("Valid license ID is required.");

            LicenseService license = await LicenseService.FindByIDAsync(licenseId);
            if (license == null)
                return Result<bool>.Fail("License does not exist.");
            if (!license.IsActive)
                return Result<bool>.Fail("Only active licenses can be detained.");
            if (await DetainedLicensesDataAccess.GetOpenByLicenseIdAsync(licenseId) != null)
                return Result<bool>.Fail("License is already detained.");

            return Result<bool>.Ok(true, "License can be detained.");
        }

        public static async Task<Result<bool>> CanReleaseDetainedLicenseAsync(int licenseId)
        {
            if (licenseId <= 0)
                return Result<bool>.Fail("Valid license ID is required.");
            if (await DetainedLicensesDataAccess.GetOpenByLicenseIdAsync(licenseId) == null)
                return Result<bool>.Fail("No open detention record was found for this license.");
            return Result<bool>.Ok(true, "Detained license can be released.");
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await DetainedLicensesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await DetainedLicensesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await DetainedLicensesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
