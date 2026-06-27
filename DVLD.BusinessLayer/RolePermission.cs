using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class RolePermissionService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }

        private RolePermissionService()
        {
            ID = -1;
            RoleID = -1;
            PermissionID = -1;
            Mode = enMode.AddNew;
        }

        private RolePermissionService(RolePermissionData data)
        {
            ID = data.ID;
            RoleID = data.RoleID;
            PermissionID = data.PermissionID;
            Mode = enMode.Update;
        }

        private RolePermissionData MapToData()
        {
            return new RolePermissionData
            {
                ID = ID,
                RoleID = RoleID,
                PermissionID = PermissionID,
            };
        }

        private Result<bool> Validate()
        {
            if (RoleID <= 0) return Result<bool>.Fail("Valid role ID is required.");
            if (PermissionID <= 0) return Result<bool>.Fail("Valid permission ID is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<RolePermissionService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await RolePermissionsDataAccess.GetByIDAsync(id);
            return data != null ? new RolePermissionService(data) : null;
        }

        public static async Task<RolePermissionService> FindByRoleIDAsync(int roleid)
        {
            if (roleid <= 0) return null;
            var list = await RolePermissionsDataAccess.GetByRoleIdAsync(roleid);
            var data = list.FirstOrDefault(x => x.RoleID == roleid);
            return data != null ? new RolePermissionService(data) : null;
        }

        public static async Task<List<RolePermissionService>> GetAllAsync()
        {
            var dataList = await RolePermissionsDataAccess.GetAllAsync();
            var list = new List<RolePermissionService>();
            foreach (var d in dataList)
                list.Add(new RolePermissionService(d));
            return list;
        }

        public static async Task<List<RolePermissionService>> GetRolePermissionsAsync(int roleId)
        {
            List<RolePermissionData> dataList = await RolePermissionsDataAccess.GetByRoleIdAsync(roleId);
            return dataList.Select(x => new RolePermissionService(x)).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await RolePermissionsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await RolePermissionsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await RolePermissionsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
