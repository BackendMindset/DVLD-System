using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class TestService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int TestAppointmentID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte TestResult { get; set; }
        public string Notes { get; set; }

        private TestService()
        {
            ID = -1;
            TestAppointmentID = -1;
            CreatedByUserID = -1;
            CreatedAt = DateTime.MinValue;
            TestResult = 0;
            Notes = string.Empty;
            Mode = enMode.AddNew;
        }

        private TestService(TestData data)
        {
            ID = data.ID;
            TestAppointmentID = data.TestAppointmentID;
            CreatedByUserID = data.CreatedByUserID;
            CreatedAt = data.CreatedAt;
            TestResult = data.TestResult;
            Notes = data.Notes ?? string.Empty;
            Mode = enMode.Update;
        }

        private TestData MapToData()
        {
            return new TestData
            {
                ID = ID,
                TestAppointmentID = TestAppointmentID,
                CreatedByUserID = CreatedByUserID,
                CreatedAt = CreatedAt,
                TestResult = TestResult,
                Notes = Notes?.Trim(),
            };
        }

        private Result<bool> Validate()
        {
            if (TestAppointmentID <= 0) return Result<bool>.Fail("Valid test appointment ID is required.");
            if (TestResult != 0 && TestResult != 1) return Result<bool>.Fail("Test result must be 0 (Fail) or 1 (Pass).");
            return Result<bool>.Ok(true);
        }

        public static async Task<TestService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await TestsDataAccess.GetByIDAsync(id);
            return data != null ? new TestService(data) : null;
        }

        public static async Task<TestService> FindByTestAppointmentIDAsync(int testappointmentid)
        {
            if (testappointmentid <= 0) return null;
            var list = await TestsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.TestAppointmentID == testappointmentid);
            return data != null ? new TestService(data) : null;
        }

        public static async Task<List<TestService>> GetAllAsync()
        {
            var dataList = await TestsDataAccess.GetAllAsync();
            var list = new List<TestService>();
            foreach (var d in dataList)
                list.Add(new TestService(d));
            return list;
        }

        public static async Task<List<TestService>> GetTestsByApplicationIdAsync(int applicationId)
        {
            List<TestData> dataList = await TestsDataAccess.GetByApplicationIdAsync(applicationId);
            return dataList.Select(x => new TestService(x)).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await TestsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await TestsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await TestsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
