using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class PermissionService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }

        private PermissionService()
        {
            ID = -1;
            PermissionName = string.Empty;
            Description = string.Empty;
            Mode = enMode.AddNew;
        }

        private PermissionService(PermissionData data)
        {
            ID = data.ID;
            PermissionName = data.PermissionName ?? string.Empty;
            Description = data.Description ?? string.Empty;
            Mode = enMode.Update;
        }

        private PermissionData MapToData()
        {
            return new PermissionData
            {
                ID = ID,
                PermissionName = PermissionName?.Trim(),
                Description = Description?.Trim(),
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(PermissionName)) return Result<bool>.Fail("Permission name is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<PermissionService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await PermissionsDataAccess.GetByIDAsync(id);
            return data != null ? new PermissionService(data) : null;
        }

        public static async Task<List<PermissionService>> GetAllAsync()
        {
            var dataList = await PermissionsDataAccess.GetAllAsync();
            var list = new List<PermissionService>();
            foreach (var d in dataList)
                list.Add(new PermissionService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await PermissionsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await PermissionsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await PermissionsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
