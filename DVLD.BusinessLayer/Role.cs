using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class RoleService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        private RoleService()
        {
            ID = -1;
            RoleName = string.Empty;
            Description = string.Empty;
            IsActive = false;
            Mode = enMode.AddNew;
        }

        private RoleService(RoleData data)
        {
            ID = data.ID;
            RoleName = data.RoleName ?? string.Empty;
            Description = data.Description ?? string.Empty;
            IsActive = data.IsActive;
            Mode = enMode.Update;
        }

        private RoleData MapToData()
        {
            return new RoleData
            {
                ID = ID,
                RoleName = RoleName?.Trim(),
                Description = Description?.Trim(),
                IsActive = IsActive,
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(RoleName)) return Result<bool>.Fail("Role name is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<RoleService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await RolesDataAccess.GetByIDAsync(id);
            return data != null ? new RoleService(data) : null;
        }

        public static async Task<List<RoleService>> GetAllAsync()
        {
            var dataList = await RolesDataAccess.GetAllAsync();
            var list = new List<RoleService>();
            foreach (var d in dataList)
                list.Add(new RoleService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await RolesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await RolesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await RolesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
