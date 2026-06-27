using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class TestTypeService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public decimal TestTypeFees { get; set; }
        public bool IsRequired { get; set; }

        private TestTypeService()
        {
            ID = -1;
            TestTypeTitle = string.Empty;
            TestTypeDescription = string.Empty;
            TestTypeFees = 0m;
            IsRequired = false;
            Mode = enMode.AddNew;
        }

        private TestTypeService(TestTypeData data)
        {
            ID = data.ID;
            TestTypeTitle = data.TestTypeTitle ?? string.Empty;
            TestTypeDescription = data.TestTypeDescription ?? string.Empty;
            TestTypeFees = data.TestTypeFees;
            IsRequired = data.IsRequired;
            Mode = enMode.Update;
        }

        private TestTypeData MapToData()
        {
            return new TestTypeData
            {
                ID = ID,
                TestTypeTitle = TestTypeTitle?.Trim(),
                TestTypeDescription = TestTypeDescription?.Trim(),
                TestTypeFees = TestTypeFees,
                IsRequired = IsRequired,
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(TestTypeTitle)) return Result<bool>.Fail("Test type title is required.");
            if (TestTypeFees < 0) return Result<bool>.Fail("Test fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<TestTypeService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await TestTypesDataAccess.GetByIDAsync(id);
            return data != null ? new TestTypeService(data) : null;
        }

        public static async Task<List<TestTypeService>> GetAllAsync()
        {
            var dataList = await TestTypesDataAccess.GetAllAsync();
            var list = new List<TestTypeService>();
            foreach (var d in dataList)
                list.Add(new TestTypeService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await TestTypesDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await TestTypesDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await TestTypesDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
