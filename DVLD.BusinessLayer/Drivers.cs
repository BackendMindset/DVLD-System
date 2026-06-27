using DVLD.DataAccess;
using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public class DriverService
    {
        public int ID { get; private set; }
        public int PersonID { get; private set; }
        public int CreatedByUserID { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public bool IsFound { get; private set; }

        private DriverService()
        {
            ID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.MinValue;
            IsFound = false;
        }

        private DriverService(DriverData data)
        {
            ID = data.ID;
            PersonID = data.PersonID;
            CreatedByUserID = data.CreatedByUserID;
            CreatedDate = data.CreatedDate;
            IsFound = data.IsFound;
        }
        public static async Task<DriverService> FindByIDAsync(int id)
        {
            var data = await DriversDataAccess.GetByIDAsync(id);
            return data.IsFound ? new DriverService(data) : new DriverService();
        }
        public static async Task<DriverService> FindByPersonIDAsync(int personID)
        {
            var data = await DriversDataAccess.GetByPersonIDAsync(personID);
            return data.IsFound ? new DriverService(data) : new DriverService();
        }
        public static async Task<bool> IsDriverExistAsync(int personID)
        {
            return await DriversDataAccess.ExistsByPersonIDAsync(personID);
        }

        public static async Task<Result<int>> CreateDriverAsync(int personID, int createdByUserID)
        {
            Result<bool> validation = await ValidateDriverAsync(personID, createdByUserID);
            if (!validation.Success)
                return Result<int>.Fail(validation.Message);

            DriverData driver = new DriverData
            {
                PersonID = personID,
                CreatedByUserID = createdByUserID
            };

            int id = await DriversDataAccess.AddAsync(driver);
            if (id == -1) return Result<int>.Fail("Failed to create driver in database.");
            return Result<int>.Ok(id, "Driver created successfully.");
        }

        public static async Task<Result<bool>> ValidateDriverAsync(int personID, int createdByUserID)
        {
            if (personID <= 0 || createdByUserID <= 0)
                return Result<bool>.Fail("Invalid person ID or user ID.");
            if (!await PersonsDataAccess.IsPersonExistAsync(personID))
                return Result<bool>.Fail("Person does not exist.");
            if (!await UsersDataAccess.ExistsAsync(createdByUserID))
                return Result<bool>.Fail("Creator user does not exist.");
            if (await DriversDataAccess.ExistsByPersonIDAsync(personID))
                return Result<bool>.Fail("Driver already exists for this person.");
            return Result<bool>.Ok(true);
        }

        public static async Task<List<DriverListDto>> SearchDriversAsync(string value)
        {
            List<DriverListData> data = await DriversDataAccess.SearchDriversAsync(value);
            return data.Select(x => new DriverListDto
            {
                DriverID = x.DriverID,
                PersonID = x.PersonID,
                NationalID = x.NationalID,
                FullName = x.FullName,
                CreatedDate = x.CreatedDate
            }).ToList();
        }

        public static async Task<List<DriverListDto>> GetDriversForListAsync()
        {
            List<DriverListData> data = await DriversDataAccess.GetDriversForListAsync();
            return data.Select(x => new DriverListDto
            {
                DriverID = x.DriverID,
                PersonID = x.PersonID,
                NationalID = x.NationalID,
                FullName = x.FullName,
                CreatedDate = x.CreatedDate
            }).ToList();
        }

        public static async Task<List<DriverService>> GetAllAsync()
        {
            var dataList = await DriversDataAccess.GetAllAsync();
            var list = new List<DriverService>();

            foreach (var d in dataList)
                list.Add(new DriverService(d));

            return list;
        }
        public async Task<Result<bool>> DeleteAsync()
        {
            if (this.ID == -1) return Result<bool>.Fail("Invalid driver ID.");
            bool deleted = await DriversDataAccess.DeleteAsync(this.ID);
            if (!deleted) return Result<bool>.Fail("Failed to delete driver from database.");
            return Result<bool>.Ok(true, "Driver deleted successfully.");
        }
    }
}
