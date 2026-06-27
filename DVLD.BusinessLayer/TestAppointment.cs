using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class TestAppointmentService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int TestTypeID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsLocked { get; set; }

        private TestAppointmentService()
        {
            ID = -1;
            LocalDrivingLicenseApplicationID = -1;
            TestTypeID = -1;
            CreatedByUserID = -1;
            AppointmentDate = DateTime.MinValue;
            PaidFees = 0m;
            IsLocked = false;
            Mode = enMode.AddNew;
        }

        private TestAppointmentService(TestAppointmentData data)
        {
            ID = data.ID;
            LocalDrivingLicenseApplicationID = data.LocalDrivingLicenseApplicationID;
            TestTypeID = data.TestTypeID;
            CreatedByUserID = data.CreatedByUserID;
            AppointmentDate = data.AppointmentDate;
            PaidFees = data.PaidFees;
            IsLocked = data.IsLocked;
            Mode = enMode.Update;
        }

        private TestAppointmentData MapToData()
        {
            return new TestAppointmentData
            {
                ID = ID,
                LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID,
                TestTypeID = TestTypeID,
                CreatedByUserID = CreatedByUserID,
                AppointmentDate = AppointmentDate,
                PaidFees = PaidFees,
                IsLocked = IsLocked,
            };
        }

        private Result<bool> Validate()
        {
            if (LocalDrivingLicenseApplicationID <= 0) return Result<bool>.Fail("Valid local driving license application ID is required.");
            if (TestTypeID <= 0) return Result<bool>.Fail("Valid test type ID is required.");
            if (AppointmentDate < DateTime.Now) return Result<bool>.Fail("Appointment date cannot be in the past.");
            if (PaidFees < 0) return Result<bool>.Fail("Paid fees cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<TestAppointmentService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await TestAppointmentsDataAccess.GetByIDAsync(id);
            return data != null ? new TestAppointmentService(data) : null;
        }

        public static async Task<TestAppointmentService> FindByLDLAAsync(int localdrivinglicenseapplicationid)
        {
            if (localdrivinglicenseapplicationid <= 0) return null;
            var list = await TestAppointmentsDataAccess.GetByLocalDrivingLicenseApplicationIdAsync(localdrivinglicenseapplicationid);
            var data = list.FirstOrDefault(x => x.LocalDrivingLicenseApplicationID == localdrivinglicenseapplicationid);
            return data != null ? new TestAppointmentService(data) : null;
        }

        public static async Task<List<TestAppointmentService>> GetAllAsync()
        {
            var dataList = await TestAppointmentsDataAccess.GetAllAsync();
            var list = new List<TestAppointmentService>();
            foreach (var d in dataList)
                list.Add(new TestAppointmentService(d));
            return list;
        }

        public static async Task<Result<bool>> CanScheduleTestAsync(int localDrivingLicenseApplicationId, int testTypeId, DateTime appointmentDate)
        {
            if (localDrivingLicenseApplicationId <= 0)
                return Result<bool>.Fail("Valid local driving license application ID is required.");
            if (testTypeId <= 0)
                return Result<bool>.Fail("Valid test type ID is required.");
            if (appointmentDate < DateTime.Now)
                return Result<bool>.Fail("Appointment date cannot be in the past.");
            if (await TestAppointmentsDataAccess.HasUnlockedAppointmentAsync(localDrivingLicenseApplicationId, testTypeId))
                return Result<bool>.Fail("There is already an unlocked appointment for this test type.");
            return Result<bool>.Ok(true, "Test can be scheduled.");
        }

        public static async Task<Result<bool>> CanRetakeTestAsync(int localDrivingLicenseApplicationId, int testTypeId)
        {
            if (await TestAppointmentsDataAccess.HasUnlockedAppointmentAsync(localDrivingLicenseApplicationId, testTypeId))
                return Result<bool>.Fail("Cannot retake the test while another appointment is still open.");
            return Result<bool>.Ok(true, "Test can be retaken.");
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await TestAppointmentsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await TestAppointmentsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await TestAppointmentsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
