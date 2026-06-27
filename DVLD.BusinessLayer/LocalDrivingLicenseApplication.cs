using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class LocalDrivingLicenseApplicationService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public string Notes { get; set; }

        private LocalDrivingLicenseApplicationService()
        {
            ID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
            Notes = string.Empty;
            Mode = enMode.AddNew;
        }

        private LocalDrivingLicenseApplicationService(LocalDrivingLicenseApplicationData data)
        {
            ID = data.ID;
            ApplicationID = data.ApplicationID;
            LicenseClassID = data.LicenseClassID;
            Notes = data.Notes ?? string.Empty;
            Mode = enMode.Update;
        }

        private LocalDrivingLicenseApplicationData MapToData()
        {
            return new LocalDrivingLicenseApplicationData
            {
                ID = ID,
                ApplicationID = ApplicationID,
                LicenseClassID = LicenseClassID,
                Notes = Notes?.Trim(),
            };
        }

        private Result<bool> Validate()
        {
            if (ApplicationID <= 0) return Result<bool>.Fail("Valid application ID is required.");
            if (LicenseClassID <= 0) return Result<bool>.Fail("Valid license class ID is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<LocalDrivingLicenseApplicationService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await LocalDrivingLicenseApplicationsDataAccess.GetByIDAsync(id);
            return data != null ? new LocalDrivingLicenseApplicationService(data) : null;
        }

        public static async Task<LocalDrivingLicenseApplicationService> FindByApplicationIDAsync(int applicationid)
        {
            if (applicationid <= 0) return null;
            var list = await LocalDrivingLicenseApplicationsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.ApplicationID == applicationid);
            return data != null ? new LocalDrivingLicenseApplicationService(data) : null;
        }

        public static async Task<List<LocalDrivingLicenseApplicationService>> GetAllAsync()
        {
            var dataList = await LocalDrivingLicenseApplicationsDataAccess.GetAllAsync();
            var list = new List<LocalDrivingLicenseApplicationService>();
            foreach (var d in dataList)
                list.Add(new LocalDrivingLicenseApplicationService(d));
            return list;
        }

        public static async Task<List<LocalDrivingLicenseApplicationListDto>> GetForListAsync()
        {
            List<LocalDrivingLicenseApplicationListData> dataList = await LocalDrivingLicenseApplicationsDataAccess.GetForListAsync();
            return dataList.Select(x => new LocalDrivingLicenseApplicationListDto
            {
                LocalDrivingLicenseApplicationID = x.LocalDrivingLicenseApplicationID,
                ApplicationID = x.ApplicationID,
                LicenseClassID = x.LicenseClassID,
                Notes = x.Notes
            }).ToList();
        }

        public static async Task<List<LocalDrivingLicenseApplicationListDto>> SearchAsync(string value)
        {
            List<LocalDrivingLicenseApplicationListData> dataList = await LocalDrivingLicenseApplicationsDataAccess.SearchAsync(value);
            return dataList.Select(x => new LocalDrivingLicenseApplicationListDto
            {
                LocalDrivingLicenseApplicationID = x.LocalDrivingLicenseApplicationID,
                ApplicationID = x.ApplicationID,
                LicenseClassID = x.LicenseClassID,
                Notes = x.Notes
            }).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await LocalDrivingLicenseApplicationsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await LocalDrivingLicenseApplicationsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await LocalDrivingLicenseApplicationsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
