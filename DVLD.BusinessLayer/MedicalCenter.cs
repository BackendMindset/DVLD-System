using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class MedicalCenterService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        private MedicalCenterService()
        {
            ID = -1;
            CenterName = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            IsActive = false;
            Mode = enMode.AddNew;
        }

        private MedicalCenterService(MedicalCenterData data)
        {
            ID = data.ID;
            CenterName = data.CenterName ?? string.Empty;
            Address = data.Address ?? string.Empty;
            Phone = data.Phone ?? string.Empty;
            IsActive = data.IsActive;
            Mode = enMode.Update;
        }

        private MedicalCenterData MapToData()
        {
            return new MedicalCenterData
            {
                ID = ID,
                CenterName = CenterName?.Trim(),
                Address = Address?.Trim(),
                Phone = Phone?.Trim(),
                IsActive = IsActive,
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(CenterName)) return Result<bool>.Fail("Center name is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<MedicalCenterService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await MedicalCentersDataAccess.GetByIDAsync(id);
            return data != null ? new MedicalCenterService(data) : null;
        }

        public static async Task<List<MedicalCenterService>> GetAllAsync()
        {
            var dataList = await MedicalCentersDataAccess.GetAllAsync();
            var list = new List<MedicalCenterService>();
            foreach (var d in dataList)
                list.Add(new MedicalCenterService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await MedicalCentersDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await MedicalCentersDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await MedicalCentersDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
