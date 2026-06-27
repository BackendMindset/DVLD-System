using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class UserRoleService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }

        private UserRoleService()
        {
            ID = -1;
            UserID = -1;
            RoleID = -1;
            Mode = enMode.AddNew;
        }

        private UserRoleService(UserRoleData data)
        {
            ID = data.ID;
            UserID = data.UserID;
            RoleID = data.RoleID;
            Mode = enMode.Update;
        }

        private UserRoleData MapToData()
        {
            return new UserRoleData
            {
                ID = ID,
                UserID = UserID,
                RoleID = RoleID,
            };
        }

        private Result<bool> Validate()
        {
            if (UserID <= 0) return Result<bool>.Fail("Valid user ID is required.");
            if (RoleID <= 0) return Result<bool>.Fail("Valid role ID is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<UserRoleService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await UserRolesDataAccess.GetByIDAsync(id);
            return data != null ? new UserRoleService(data) : null;
        }

        public static async Task<UserRoleService> FindByUserIDAsync(int userid)
        {
            if (userid <= 0) return null;
            var list = await UserRolesDataAccess.GetByUserIdAsync(userid);
            var data = list.FirstOrDefault(x => x.UserID == userid);
            return data != null ? new UserRoleService(data) : null;
        }

        public static async Task<List<UserRoleService>> GetAllAsync()
        {
            var dataList = await UserRolesDataAccess.GetAllAsync();
            var list = new List<UserRoleService>();
            foreach (var d in dataList)
                list.Add(new UserRoleService(d));
            return list;
        }

        public static async Task<List<UserRoleService>> GetUserRolesAsync(int userId)
        {
            List<UserRoleData> dataList = await UserRolesDataAccess.GetByUserIdAsync(userId);
            return dataList.Select(x => new UserRoleService(x)).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await UserRolesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await UserRolesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await UserRolesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
