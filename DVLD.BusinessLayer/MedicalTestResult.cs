using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public sealed class MedicalTestResultService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ID { get; private set; }
        public int RequestMedicalTestID { get; set; }
        public bool Result { get; set; }
        public string ResultDetails { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedByUserID { get; set; }

        private MedicalTestResultService()
        {
            ID = -1;
            RequestMedicalTestID = -1;
            Result = false;
            ResultDetails = string.Empty;
            CreatedAt = DateTime.MinValue;
            CreatedByUserID = null;
            Mode = enMode.AddNew;
        }

        private MedicalTestResultService(MedicalTestResultData data)
        {
            ID = data.ID;
            RequestMedicalTestID = data.RequestMedicalTestID;
            Result = data.Result;
            ResultDetails = data.ResultDetails ?? string.Empty;
            CreatedAt = data.CreatedAt;
            CreatedByUserID = data.CreatedByUserID;
            Mode = enMode.Update;
        }

        private MedicalTestResultData MapToData()
        {
            return new MedicalTestResultData
            {
                ID = ID,
                RequestMedicalTestID = RequestMedicalTestID,
                Result = Result,
                ResultDetails = ResultDetails?.Trim(),
                CreatedAt = CreatedAt,
                CreatedByUserID = CreatedByUserID,
            };
        }

        private Result<bool> Validate()
        {
            if (RequestMedicalTestID <= 0) return Result<bool>.Fail("Valid request medical test ID is required.");
            return Result<bool>.Ok(true);
        }

        public static async Task<MedicalTestResultService> FindByIDAsync(int id)
        {
            if (id <= 0) return null;
            var data = await MedicalTestResultsDataAccess.GetByIDAsync(id);
            return data != null ? new MedicalTestResultService(data) : null;
        }

        public static async Task<MedicalTestResultService> FindByRequestIDAsync(int requestmedicaltestid)
        {
            if (requestmedicaltestid <= 0) return null;
            var list = await MedicalTestResultsDataAccess.GetAllAsync();
            var data = list.FirstOrDefault(x => x.RequestMedicalTestID == requestmedicaltestid);
            return data != null ? new MedicalTestResultService(data) : null;
        }

        public static async Task<List<MedicalTestResultService>> GetAllAsync()
        {
            var dataList = await MedicalTestResultsDataAccess.GetAllAsync();
            var list = new List<MedicalTestResultService>();
            foreach (var d in dataList)
                list.Add(new MedicalTestResultService(d));
            return list;
        }

        public async Task<Result<bool>> SaveAsync()
        {
            var validation = Validate();
            if (!validation.Success) return validation;
            if (Mode == enMode.AddNew)
            {
                int newId = await MedicalTestResultsDataAccess.AddAsync(MapToData());
                if (newId == -1) return Result<bool>.Fail("Failed to create record in database.");
                ID = newId;
                Mode = enMode.Update;
                return Result<bool>.Ok(true, "Record created successfully.");
            }
            else
            {
                bool updated = await MedicalTestResultsDataAccess.UpdateAsync(MapToData());
                if (!updated) return Result<bool>.Fail("Failed to update record in database.");
                return Result<bool>.Ok(true, "Record updated successfully.");
            }
        }

        public static async Task<Result<bool>> DeleteAsync(int id)
        {
            if (id <= 0) return Result<bool>.Fail("Invalid ID.");
            bool deleted = await MedicalTestResultsDataAccess.DeleteAsync(id);
            if (!deleted) return Result<bool>.Fail("Failed to delete record from database.");
            return Result<bool>.Ok(true, "Record deleted successfully.");
        }
    }
}
