using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class LocalDrivingLicenseApplicationsDataAccess
    {
        private static LocalDrivingLicenseApplicationData Map(SqlDataReader reader)
        {
            return new LocalDrivingLicenseApplicationData
            {
                ID = (int)reader["ID"],
                ApplicationID = (int)reader["ApplicationID"],
                LicenseClassID = (int)reader["LicenseClassID"],
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
            };
        }

        private static LocalDrivingLicenseApplicationListData MapList(SqlDataReader reader)
        {
            return new LocalDrivingLicenseApplicationListData
            {
                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
                ApplicationID = (int)reader["ApplicationID"],
                LicenseClassID = (int)reader["LicenseClassID"],
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : reader["Notes"].ToString()
            };
        }

        public static async Task<LocalDrivingLicenseApplicationData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM LocalDrivingLicenseApplications WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                        return Map(reader);
                    return null;
                }
            }
        }

        public static async Task<List<LocalDrivingLicenseApplicationData>> GetAllAsync()
        {
            List<LocalDrivingLicenseApplicationData> list = new List<LocalDrivingLicenseApplicationData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM LocalDrivingLicenseApplications", connection))
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

        public static async Task<List<LocalDrivingLicenseApplicationListData>> GetForListAsync()
        {
            List<LocalDrivingLicenseApplicationListData> list = new List<LocalDrivingLicenseApplicationListData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT ID AS LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID, Notes
                                                         FROM LocalDrivingLicenseApplications
                                                         ORDER BY ID DESC", connection))
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

        public static async Task<List<LocalDrivingLicenseApplicationListData>> SearchAsync(string value)
        {
            List<LocalDrivingLicenseApplicationListData> list = new List<LocalDrivingLicenseApplicationListData>();
            if (string.IsNullOrWhiteSpace(value))
                return list;

            bool isId = int.TryParse(value.Trim(), out int id);
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"SELECT ID AS LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID, Notes
                  FROM LocalDrivingLicenseApplications
                  WHERE (@ID IS NOT NULL AND (ID = @ID OR ApplicationID = @ID OR LicenseClassID = @ID))
                     OR Notes LIKE @Value
                  ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = isId ? (object)id : DBNull.Value;
                command.Parameters.Add("@Value", SqlDbType.NVarChar, 255).Value = "%" + value.Trim() + "%";
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(MapList(reader));
                }
            }

            return list;
        }

        public static async Task<int> AddAsync(LocalDrivingLicenseApplicationData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID, Notes) VALUES (@ApplicationID, @LicenseClassID, @Notes); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = data.LicenseClassID;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 255).Value = data.Notes ?? (object)DBNull.Value;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(LocalDrivingLicenseApplicationData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE LocalDrivingLicenseApplications SET ApplicationID = @ApplicationID, LicenseClassID = @LicenseClassID, Notes = @Notes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = data.LicenseClassID;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 255).Value = data.Notes ?? (object)DBNull.Value;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM LocalDrivingLicenseApplications WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM LocalDrivingLicenseApplications WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
