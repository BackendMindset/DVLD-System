using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class LicenseClassService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte ValidityLength { get; set; }
        public decimal LicenseFee { get; set; }

        private LicenseClassService()
        {
            ID = -1;
            ClassName = string.Empty;
            ClassDescription = string.Empty;
            MinimumAllowedAge = 0;
            ValidityLength = 0;
            LicenseFee = 0m;
            Mode = enMode.AddNew;
        }

        private LicenseClassService(LicenseClassData data)
        {
            ID = data.ID;
            ClassName = data.ClassName ?? string.Empty;
            ClassDescription = data.ClassDescription ?? string.Empty;
            MinimumAllowedAge = data.MinimumAllowedAge;
            ValidityLength = data.ValidityLength;
            LicenseFee = data.LicenseFee;
            Mode = enMode.Update;
        }

        private LicenseClassData MapToData()
        {
            return new LicenseClassData
            {
                ID = ID,
                ClassName = ClassName?.Trim(),
                ClassDescription = ClassDescription?.Trim(),
                MinimumAllowedAge = MinimumAllowedAge,
                ValidityLength = ValidityLength,
                LicenseFee = LicenseFee,
            };
        }

        private Result<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(ClassName)) return Result<bool>.Fail("Class name is required.");
            if (MinimumAllowedAge < 16) return Result<bool>.Fail("Minimum allowed age must be at least 16.");
            if (ValidityLength < 1) return Result<bool>.Fail("Validity length must be at least 1 year.");
            if (LicenseFee < 0) return Result<bool>.Fail("License fee cannot be negative.");
            return Result<bool>.Ok(true);
        }

        public static async Task<LicenseClassService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await LicenseClasssDataAccess.GetByIDAsync(id);
            return data != null ? new LicenseClassService(data) : null;
        }

        public static async Task<List<LicenseClassService>> GetAllAsync()
        {
            var dataList = await LicenseClasssDataAccess.GetAllAsync();
            var list = new List<LicenseClassService>();
            foreach (var d in dataList)
                list.Add(new LicenseClassService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await LicenseClasssDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await LicenseClasssDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await LicenseClasssDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
