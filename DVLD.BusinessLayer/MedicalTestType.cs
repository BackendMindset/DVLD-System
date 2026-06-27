using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class MedicalTestTypeService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string TestName { get; set; }
        public string Description { get; set; }
        public decimal DefaultFee { get; set; }
        public bool IsRequired { get; set; }

        private MedicalTestTypeService()
        {
            ID = -1;
            TestName = string.Empty;
            Description = string.Empty;
            DefaultFee = 0m;
            IsRequired = false;
            Mode = enMode.AddNew;
        }

        private MedicalTestTypeService(MedicalTestTypeData data)
        {
            ID = data.ID;
            TestName = data.TestName ?? string.Empty;
            Description = data.Description ?? string.Empty;
            DefaultFee = data.DefaultFee;
            IsRequired = data.IsRequired;
            Mode = enMode.Update;
        }

        private MedicalTestTypeData MapToData()
        {
            return new MedicalTestTypeData
            {
                ID = ID,
                TestName = TestName?.Trim(),
                Description = Description?.Trim(),
                DefaultFee = DefaultFee,
                IsRequired = IsRequired,
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(TestName)) return Result<bool>.Fail("Test name is required.");
            if (DefaultFee < 0) return Result<bool>.Fail("Default fee cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<MedicalTestTypeService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await MedicalTestTypesDataAccess.GetByIDAsync(id);
            return data != null ? new MedicalTestTypeService(data) : null;
        }

        public static async Task<List<MedicalTestTypeService>> GetAllAsync()
        {
            var dataList = await MedicalTestTypesDataAccess.GetAllAsync();
            var list = new List<MedicalTestTypeService>();
            foreach (var d in dataList)
                list.Add(new MedicalTestTypeService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await MedicalTestTypesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await MedicalTestTypesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await MedicalTestTypesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
