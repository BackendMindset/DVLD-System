using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class MedicalCenterTestService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int CenterID { get; set; }
        public int TestTypeID { get; set; }
        public decimal Fee { get; set; }

        private MedicalCenterTestService()
        {
            ID = -1;
            CenterID = -1;
            TestTypeID = -1;
            Fee = 0m;
            Mode = enMode.AddNew;
        }

        private MedicalCenterTestService(MedicalCenterTestData data)
        {
            ID = data.ID;
            CenterID = data.CenterID;
            TestTypeID = data.TestTypeID;
            Fee = data.Fee;
            Mode = enMode.Update;
        }

        private MedicalCenterTestData MapToData()
        {
            return new MedicalCenterTestData
            {
                ID = ID,
                CenterID = CenterID,
                TestTypeID = TestTypeID,
                Fee = Fee,
            };
        }

        private Result<bool> Validate()
        {
            if (CenterID <= 0) return Result<bool>.Fail("Valid center ID is required.");
            if (TestTypeID <= 0) return Result<bool>.Fail("Valid test type ID is required.");
            if (Fee < 0) return Result<bool>.Fail("Fee cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<MedicalCenterTestService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await MedicalCenterTestsDataAccess.GetByIDAsync(id);
            return data != null ? new MedicalCenterTestService(data) : null;
        }

        public static async Task<MedicalCenterTestService> FindByCenterIDAsync(int centerid)
        {
            if (centerid <= 0) return null;
            var list = await MedicalCenterTestsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.CenterID == centerid);
            return data != null ? new MedicalCenterTestService(data) : null;
        }

        public static async Task<List<MedicalCenterTestService>> GetAllAsync()
        {
            var dataList = await MedicalCenterTestsDataAccess.GetAllAsync();
            var list = new List<MedicalCenterTestService>();
            foreach (var d in dataList)
                list.Add(new MedicalCenterTestService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await MedicalCenterTestsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await MedicalCenterTestsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await MedicalCenterTestsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
