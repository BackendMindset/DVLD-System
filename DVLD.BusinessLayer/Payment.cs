using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class PaymentService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ApplicationID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime PaymentDate { get; set; }

        private PaymentService()
        {
            ID = -1;
            ApplicationID = -1;
            Amount = 0m;
            PaymentMethod = string.Empty;
            Status = string.Empty;
            ReferenceNumber = string.Empty;
            PaymentDate = DateTime.MinValue;
            Mode = enMode.AddNew;
        }

        private PaymentService(PaymentData data)
        {
            ID = data.ID;
            ApplicationID = data.ApplicationID;
            Amount = data.Amount;
            PaymentMethod = data.PaymentMethod ?? string.Empty;
            Status = data.Status ?? string.Empty;
            ReferenceNumber = data.ReferenceNumber ?? string.Empty;
            PaymentDate = data.PaymentDate;
            Mode = enMode.Update;
        }

        private PaymentData MapToData()
        {
            return new PaymentData
            {
                ID = ID,
                ApplicationID = ApplicationID,
                Amount = Amount,
                PaymentMethod = PaymentMethod?.Trim(),
                Status = Status?.Trim(),
                ReferenceNumber = ReferenceNumber?.Trim(),
                PaymentDate = PaymentDate,
            };
        }

        private Result<bool> Validate()
        {
            if (ApplicationID <= 0) return Result<bool>.Fail("Valid application ID is required.");
            if (Amount < 0) return Result<bool>.Fail("Amount cannot be negative.");
            if (string.IsNullOrWhiteSpace(PaymentMethod)) return Result<bool>.Fail("Payment method is required.");
            if (string.IsNullOrWhiteSpace(ReferenceNumber)) return Result<bool>.Fail("Reference number is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<PaymentService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await PaymentsDataAccess.GetByIDAsync(id);
            return data != null ? new PaymentService(data) : null;
        }

        public static async Task<PaymentService> FindByApplicationIDAsync(int applicationid)
        {
            if (applicationid <= 0) return null;
            var list = await PaymentsDataAccess.GetByApplicationIdAsync(applicationid);
            var data = list.FirstOrDefault(x => x.ApplicationID == applicationid);
            return data != null ? new PaymentService(data) : null;
        }

        public static async Task<List<PaymentService>> GetAllAsync()
        {
            var dataList = await PaymentsDataAccess.GetAllAsync();
            var list = new List<PaymentService>();
            foreach (var d in dataList)
                list.Add(new PaymentService(d));
            return list;
        }

        public static async Task<List<PaymentService>> GetPaymentsByApplicationIdAsync(int applicationId)
        {
            List<PaymentData> dataList = await PaymentsDataAccess.GetByApplicationIdAsync(applicationId);
            return dataList.Select(x => new PaymentService(x)).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await PaymentsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await PaymentsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await PaymentsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
