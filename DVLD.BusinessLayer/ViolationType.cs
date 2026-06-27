using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class ViolationTypeService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string ViolationName { get; set; }
        public string Description { get; set; }
        public decimal BaseFineAmount { get; set; }

        private ViolationTypeService()
        {
            ID = -1;
            ViolationName = string.Empty;
            Description = string.Empty;
            BaseFineAmount = 0m;
            Mode = enMode.AddNew;
        }

        private ViolationTypeService(ViolationTypeData data)
        {
            ID = data.ID;
            ViolationName = data.ViolationName ?? string.Empty;
            Description = data.Description ?? string.Empty;
            BaseFineAmount = data.BaseFineAmount;
            Mode = enMode.Update;
        }

        private ViolationTypeData MapToData()
        {
            return new ViolationTypeData
            {
                ID = ID,
                ViolationName = ViolationName?.Trim(),
                Description = Description?.Trim(),
                BaseFineAmount = BaseFineAmount,
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(ViolationName)) return Result<bool>.Fail("Violation name is required.");
            if (BaseFineAmount < 0) return Result<bool>.Fail("Base fine amount cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<ViolationTypeService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await ViolationTypesDataAccess.GetByIDAsync(id);
            return data != null ? new ViolationTypeService(data) : null;
        }

        public static async Task<List<ViolationTypeService>> GetAllAsync()
        {
            var dataList = await ViolationTypesDataAccess.GetAllAsync();
            var list = new List<ViolationTypeService>();
            foreach (var d in dataList)
                list.Add(new ViolationTypeService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await ViolationTypesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await ViolationTypesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await ViolationTypesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
