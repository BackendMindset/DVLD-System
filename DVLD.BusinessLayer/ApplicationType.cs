using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class ApplicationTypeService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }

        private ApplicationTypeService()
        {
            ID = -1;
            ApplicationTypeTitle = string.Empty;
            ApplicationFees = 0m;
            Mode = enMode.AddNew;
        }

        private ApplicationTypeService(ApplicationTypeData data)
        {
            ID = data.ID;
            ApplicationTypeTitle = data.ApplicationTypeTitle ?? string.Empty;
            ApplicationFees = data.ApplicationFees;
            Mode = enMode.Update;
        }

        private ApplicationTypeData MapToData()
        {
            return new ApplicationTypeData
            {
                ID = ID,
                ApplicationTypeTitle = ApplicationTypeTitle?.Trim(),
                ApplicationFees = ApplicationFees,
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(ApplicationTypeTitle)) return Result<bool>.Fail("Application type title is required.");
            if (ApplicationFees < 0) return Result<bool>.Fail("Application fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<ApplicationTypeService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await ApplicationTypesDataAccess.GetByIDAsync(id);
            return data != null ? new ApplicationTypeService(data) : null;
        }

        public static async Task<List<ApplicationTypeService>> GetAllAsync()
        {
            var dataList = await ApplicationTypesDataAccess.GetAllAsync();
            var list = new List<ApplicationTypeService>();
            foreach (var d in dataList)
                list.Add(new ApplicationTypeService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await ApplicationTypesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await ApplicationTypesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await ApplicationTypesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
