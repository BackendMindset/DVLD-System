using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class ViolationService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int ViolationTypeID { get; set; }
        public int PersonID { get; set; }
        public int? LicenseID { get; set; }
        public string Location { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal FineAmount { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        private ViolationService()
        {
            ID = -1;
            ViolationTypeID = -1;
            PersonID = -1;
            LicenseID = null;
            Location = string.Empty;
            IssueDate = DateTime.MinValue;
            FineAmount = 0m;
            Status = string.Empty;
            Notes = string.Empty;
            Mode = enMode.AddNew;
        }

        private ViolationService(ViolationData data)
        {
            ID = data.ID;
            ViolationTypeID = data.ViolationTypeID;
            PersonID = data.PersonID;
            LicenseID = data.LicenseID;
            Location = data.Location ?? string.Empty;
            IssueDate = data.IssueDate;
            FineAmount = data.FineAmount;
            Status = data.Status ?? string.Empty;
            Notes = data.Notes ?? string.Empty;
            Mode = enMode.Update;
        }

        private ViolationData MapToData()
        {
            return new ViolationData
            {
                ID = ID,
                ViolationTypeID = ViolationTypeID,
                PersonID = PersonID,
                LicenseID = LicenseID,
                Location = Location?.Trim(),
                IssueDate = IssueDate,
                FineAmount = FineAmount,
                Status = Status?.Trim(),
                Notes = Notes?.Trim(),
            };
        }

        private Result<bool> Validate()
        {
            if (ViolationTypeID <= 0) return Result<bool>.Fail("Valid violation type ID is required.");
            if (PersonID <= 0) return Result<bool>.Fail("Valid person ID is required.");
            if (FineAmount < 0) return Result<bool>.Fail("Fine amount cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<ViolationService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await ViolationsDataAccess.GetByIDAsync(id);
            return data != null ? new ViolationService(data) : null;
        }

        public static async Task<ViolationService> FindByPersonIDAsync(int personid)
        {
            if (personid <= 0) return null;
            var list = await ViolationsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.PersonID == personid);
            return data != null ? new ViolationService(data) : null;
        }

        public static async Task<List<ViolationService>> GetAllAsync()
        {
            var dataList = await ViolationsDataAccess.GetAllAsync();
            var list = new List<ViolationService>();
            foreach (var d in dataList)
                list.Add(new ViolationService(d));
            return list;
        }

        public static async Task<List<ViolationService>> GetViolationsByDriverIdAsync(int driverId)
        {
            List<ViolationData> dataList = await ViolationsDataAccess.GetByDriverIdAsync(driverId);
            return dataList.Select(x => new ViolationService(x)).ToList();
        }

        public static async Task<List<ViolationListDto>> SearchViolationsAsync(string value)
        {
            List<ViolationListData> dataList = await ViolationsDataAccess.SearchViolationsAsync(value);
            return dataList.Select(x => new ViolationListDto
            {
                ViolationID = x.ViolationID,
                ViolationType = x.ViolationType,
                PersonName = x.PersonName,
                IssueDate = x.IssueDate,
                FineAmount = x.FineAmount,
                Status = x.Status
            }).ToList();
        }

        public static async Task<List<ViolationListDto>> GetViolationsForListAsync()
        {
            List<ViolationListData> dataList = await ViolationsDataAccess.GetViolationsForListAsync();
            return dataList.Select(x => new ViolationListDto
            {
                ViolationID = x.ViolationID,
                ViolationType = x.ViolationType,
                PersonName = x.PersonName,
                IssueDate = x.IssueDate,
                FineAmount = x.FineAmount,
                Status = x.Status
            }).ToList();
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await ViolationsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await ViolationsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await ViolationsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
