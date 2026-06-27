using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class PaymentDetailService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public string ItemName { get; set; }

        private PaymentDetailService()
        {
            ID = -1;
            PaymentID = -1;
            Amount = 0m;
            ItemName = string.Empty;
            Mode = enMode.AddNew;
        }

        private PaymentDetailService(PaymentDetailData data)
        {
            ID = data.ID;
            PaymentID = data.PaymentID;
            Amount = data.Amount;
            ItemName = data.ItemName ?? string.Empty;
            Mode = enMode.Update;
        }

        private PaymentDetailData MapToData()
        {
            return new PaymentDetailData
            {
                ID = ID,
                PaymentID = PaymentID,
                Amount = Amount,
                ItemName = ItemName?.Trim(),
            };
        }

        private Result<bool> Validate()
        {
            if (PaymentID <= 0) return Result<bool>.Fail("Valid payment ID is required.");
            if (string.IsNullOrWhiteSpace(ItemName)) return Result<bool>.Fail("Item name is required.");
            if (Amount < 0) return Result<bool>.Fail("Amount cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<PaymentDetailService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await PaymentDetailsDataAccess.GetByIDAsync(id);
            return data != null ? new PaymentDetailService(data) : null;
        }

        public static async Task<PaymentDetailService> FindByPaymentIDAsync(int paymentid)
        {
            if (paymentid <= 0) return null;
            var list = await PaymentDetailsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.PaymentID == paymentid);
            return data != null ? new PaymentDetailService(data) : null;
        }

        public static async Task<List<PaymentDetailService>> GetAllAsync()
        {
            var dataList = await PaymentDetailsDataAccess.GetAllAsync();
            var list = new List<PaymentDetailService>();
            foreach (var d in dataList)
                list.Add(new PaymentDetailService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await PaymentDetailsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await PaymentDetailsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await PaymentDetailsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
