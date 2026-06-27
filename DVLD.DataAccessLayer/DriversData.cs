using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class DriversDataAccess
    {
        private static DriverData Map(SqlDataReader reader)
        {
            return new DriverData
            {
                IsFound = true,
                ID = (int)reader["ID"],
                PersonID = (int)reader["PersonID"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
                CreatedDate = (DateTime)reader["CreatedDate"]
            };
        }

        private static DriverListData MapList(SqlDataReader reader)
        {
            return new DriverListData
            {
                DriverID = (int)reader["DriverID"],
                PersonID = (int)reader["PersonID"],
                NationalID = reader["NationalID"] as string ?? string.Empty,
                FullName = reader["FullName"] as string ?? string.Empty,
                CreatedDate = (DateTime)reader["CreatedDate"]
            };
        }

        public static async Task<DriverData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT ID, PersonID, CreatedByUserID, CreatedDate FROM Drivers WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                await connection.OpenAsync().ConfigureAwait(false);

                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                        return Map(reader);
                    else
                        return new DriverData();
                }
            }
        }

        public static async Task<DriverData> GetByPersonIDAsync(int personID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT ID, PersonID, CreatedByUserID, CreatedDate FROM Drivers WHERE PersonID = @PersonID", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personID;

                await connection.OpenAsync().ConfigureAwait(false);

                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                        return Map(reader);
                    else
                        return new DriverData();
                }
            }
        }

        public static async Task<List<DriverData>> GetAllAsync()
        {
            List<DriverData> list = new List<DriverData>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT ID, PersonID, CreatedByUserID, CreatedDate FROM Drivers", connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }

            return list;
        }

        public static async Task<List<DriverListData>> SearchDriversAsync(string value)
        {
            List<DriverListData> list = new List<DriverListData>();
            if (string.IsNullOrWhiteSpace(value))
                return list;

            bool isId = int.TryParse(value.Trim(), out int id);
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"SELECT DriverID, PersonID, NationalID, FullName, CreatedDate
                  FROM Drivers_View
                  WHERE (@ID IS NOT NULL AND (DriverID = @ID OR PersonID = @ID))
                     OR NationalID LIKE @Value
                     OR FullName LIKE @Value
                  ORDER BY DriverID DESC", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = isId ? (object)id : DBNull.Value;
                command.Parameters.Add("@Value", SqlDbType.NVarChar, 150).Value = "%" + value.Trim() + "%";
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(MapList(reader));
                }
            }

            return list;
        }

        public static async Task<List<DriverListData>> GetDriversForListAsync()
        {
            List<DriverListData> list = new List<DriverListData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"SELECT DriverID, PersonID, NationalID, FullName, CreatedDate
                  FROM Drivers_View
                  ORDER BY DriverID DESC", connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(MapList(reader));
                }
            }

            return list;
        }

        public static async Task<int> AddAsync(DriverData driver)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"INSERT INTO Drivers (PersonID, CreatedByUserID)
                  VALUES (@PersonID, @CreatedByUserID);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = driver.PersonID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = driver.CreatedByUserID;

                await connection.OpenAsync().ConfigureAwait(false);

                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);

                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "DELETE FROM Drivers WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                await connection.OpenAsync().ConfigureAwait(false);

                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsByPersonIDAsync(int personID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT 1 FROM Drivers WHERE PersonID = @PersonID", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personID;

                await connection.OpenAsync().ConfigureAwait(false);

                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
