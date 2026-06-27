using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class RequestMedicalTestService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ApplicationID { get; set; }
        public int CenterID { get; set; }
        public int TestTypeID { get; set; }
        public string Status { get; set; }
        public DateTime? ExamDate { get; set; }
        public decimal PaidFees { get; set; }

        private RequestMedicalTestService()
        {
            ID = -1;
            ApplicationID = -1;
            CenterID = -1;
            TestTypeID = -1;
            Status = string.Empty;
            ExamDate = null;
            PaidFees = 0m;
            Mode = enMode.AddNew;
        }

        private RequestMedicalTestService(RequestMedicalTestData data)
        {
            ID = data.ID;
            ApplicationID = data.ApplicationID;
            CenterID = data.CenterID;
            TestTypeID = data.TestTypeID;
            Status = data.Status ?? string.Empty;
            ExamDate = data.ExamDate;
            PaidFees = data.PaidFees;
            Mode = enMode.Update;
        }

        private RequestMedicalTestData MapToData()
        {
            return new RequestMedicalTestData
            {
                ID = ID,
                ApplicationID = ApplicationID,
                CenterID = CenterID,
                TestTypeID = TestTypeID,
                Status = Status?.Trim(),
                ExamDate = ExamDate,
                PaidFees = PaidFees,
            };
        }

        private Result<bool> Validate()
        {
            if (ApplicationID <= 0) return Result<bool>.Fail("Valid application ID is required.");
            if (CenterID <= 0) return Result<bool>.Fail("Valid center ID is required.");
            if (TestTypeID <= 0) return Result<bool>.Fail("Valid test type ID is required.");
            if (PaidFees < 0) return Result<bool>.Fail("Paid fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<RequestMedicalTestService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await RequestMedicalTestsDataAccess.GetByIDAsync(id);
            return data != null ? new RequestMedicalTestService(data) : null;
        }

        public static async Task<RequestMedicalTestService> FindByApplicationIDAsync(int applicationid)
        {
            if (applicationid <= 0) return null;
            var list = await RequestMedicalTestsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.ApplicationID == applicationid);
            return data != null ? new RequestMedicalTestService(data) : null;
        }

        public static async Task<List<RequestMedicalTestService>> GetAllAsync()
        {
            var dataList = await RequestMedicalTestsDataAccess.GetAllAsync();
            var list = new List<RequestMedicalTestService>();
            foreach (var d in dataList)
                list.Add(new RequestMedicalTestService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await RequestMedicalTestsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await RequestMedicalTestsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await RequestMedicalTestsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
