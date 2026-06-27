using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class LicenseClassTestService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int LicenseClassID { get; set; }
        public int TestTypeID { get; set; }

        private LicenseClassTestService()
        {
            ID = -1;
            LicenseClassID = -1;
            TestTypeID = -1;
            Mode = enMode.AddNew;
        }

        private LicenseClassTestService(LicenseClassTestData data)
        {
            ID = data.ID;
            LicenseClassID = data.LicenseClassID;
            TestTypeID = data.TestTypeID;
            Mode = enMode.Update;
        }

        private LicenseClassTestData MapToData()
        {
            return new LicenseClassTestData
            {
                ID = ID,
                LicenseClassID = LicenseClassID,
                TestTypeID = TestTypeID,
            };
        }

        private Result<bool> Validate()
        {
            if (LicenseClassID <= 0) return Result<bool>.Fail("Valid license class ID is required.");
            if (TestTypeID <= 0) return Result<bool>.Fail("Valid test type ID is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<LicenseClassTestService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await LicenseClassTestsDataAccess.GetByIDAsync(id);
            return data != null ? new LicenseClassTestService(data) : null;
        }

        public static async Task<LicenseClassTestService> FindByLicenseClassIDAsync(int licenseclassid)
        {
            if (licenseclassid <= 0) return null;
            var list = await LicenseClassTestsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.LicenseClassID == licenseclassid);
            return data != null ? new LicenseClassTestService(data) : null;
        }

        public static async Task<List<LicenseClassTestService>> GetAllAsync()
        {
            var dataList = await LicenseClassTestsDataAccess.GetAllAsync();
            var list = new List<LicenseClassTestService>();
            foreach (var d in dataList)
                list.Add(new LicenseClassTestService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await LicenseClassTestsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await LicenseClassTestsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await LicenseClassTestsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
