using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class ViolationPaymentService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ViolationID { get; set; }
        public int PaymentID { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidDate { get; set; }
        public string Status { get; set; }

        private ViolationPaymentService()
        {
            ID = -1;
            ViolationID = -1;
            PaymentID = -1;
            PaidAmount = 0m;
            PaidDate = DateTime.MinValue;
            Status = string.Empty;
            Mode = enMode.AddNew;
        }

        private ViolationPaymentService(ViolationPaymentData data)
        {
            ID = data.ID;
            ViolationID = data.ViolationID;
            PaymentID = data.PaymentID;
            PaidAmount = data.PaidAmount;
            PaidDate = data.PaidDate;
            Status = data.Status ?? string.Empty;
            Mode = enMode.Update;
        }

        private ViolationPaymentData MapToData()
        {
            return new ViolationPaymentData
            {
                ID = ID,
                ViolationID = ViolationID,
                PaymentID = PaymentID,
                PaidAmount = PaidAmount,
                PaidDate = PaidDate,
                Status = Status?.Trim(),
            };
        }

        private Result<bool> Validate()
        {
            if (ViolationID <= 0) return Result<bool>.Fail("Valid violation ID is required.");
            if (PaymentID <= 0) return Result<bool>.Fail("Valid payment ID is required.");
            if (PaidAmount < 0) return Result<bool>.Fail("Paid amount cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<ViolationPaymentService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await ViolationPaymentsDataAccess.GetByIDAsync(id);
            return data != null ? new ViolationPaymentService(data) : null;
        }

        public static async Task<ViolationPaymentService> FindByViolationIDAsync(int violationid)
        {
            if (violationid <= 0) return null;
            var list = await ViolationPaymentsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.ViolationID == violationid);
            return data != null ? new ViolationPaymentService(data) : null;
        }

        public static async Task<List<ViolationPaymentService>> GetAllAsync()
        {
            var dataList = await ViolationPaymentsDataAccess.GetAllAsync();
            var list = new List<ViolationPaymentService>();
            foreach (var d in dataList)
                list.Add(new ViolationPaymentService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await ViolationPaymentsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await ViolationPaymentsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await ViolationPaymentsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
