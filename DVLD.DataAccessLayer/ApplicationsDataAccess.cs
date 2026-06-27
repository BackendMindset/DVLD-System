using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class ApplicationsDataAccess
    {
        private static ApplicationData Map(SqlDataReader reader)
        {
            return new ApplicationData
            {
                ID = (int)reader["ID"],
                ApplicantPersonID = (int)reader["ApplicantPersonID"],
                ApplicationTypeID = (int)reader["ApplicationTypeID"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
                ApplicationDate = (DateTime)reader["ApplicationDate"],
                ApplicationStatus = Convert.ToByte(reader["ApplicationStatus"]),
                LastStatusDate = (DateTime)reader["LastStatusDate"],
                PaidFees = (decimal)reader["PaidFees"],
                UpdatedAt = (DateTime)reader["UpdatedAt"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                IsActive = (bool)reader["IsActive"],
            };
        }

        private static ApplicationListData MapList(SqlDataReader reader)
        {
            return new ApplicationListData
            {
                ApplicationID = (int)reader["ApplicationID"],
                ApplicantName = reader["ApplicantName"] as string ?? string.Empty,
                ApplicationType = reader["ApplicationType"] as string ?? string.Empty,
                ApplicationDate = (DateTime)reader["ApplicationDate"],
                Status = reader["Status"] as string ?? string.Empty,
                PaidFees = (decimal)reader["PaidFees"]
            };
        }

        public static async Task<ApplicationData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Applications WHERE ID = @ID", connection))
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

        public static async Task<List<ApplicationData>> GetAllAsync()
        {
            List<ApplicationData> list = new List<ApplicationData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Applications", connection))
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

        public static async Task<List<ApplicationData>> GetByPersonIdAsync(int personId)
        {
            List<ApplicationData> list = new List<ApplicationData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = personId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public static async Task<List<ApplicationListData>> SearchApplicationsAsync(string value)
        {
            List<ApplicationListData> list = new List<ApplicationListData>();
            if (string.IsNullOrWhiteSpace(value))
                return list;

            bool isId = int.TryParse(value.Trim(), out int id);
            string query = @"SELECT a.ID AS ApplicationID,
                                    p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName AS ApplicantName,
                                    at.ApplicationTypeTitle AS ApplicationType,
                                    a.ApplicationDate,
                                    CASE a.ApplicationStatus WHEN 1 THEN 'New' WHEN 2 THEN 'Cancelled' WHEN 3 THEN 'Completed' ELSE 'Unknown' END AS Status,
                                    a.PaidFees
                             FROM Applications a
                             INNER JOIN People p ON p.ID = a.ApplicantPersonID
                             INNER JOIN ApplicationTypes at ON at.ID = a.ApplicationTypeID
                             WHERE (@ID IS NOT NULL AND (a.ID = @ID OR a.ApplicantPersonID = @ID))
                                OR p.NationalID LIKE @Value
                                OR (p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName) LIKE @Value
                             ORDER BY a.ID DESC";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
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

        public static async Task<List<ApplicationListData>> GetApplicationsForListAsync()
        {
            List<ApplicationListData> list = new List<ApplicationListData>();
            string query = @"SELECT a.ID AS ApplicationID,
                                    p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName AS ApplicantName,
                                    at.ApplicationTypeTitle AS ApplicationType,
                                    a.ApplicationDate,
                                    CASE a.ApplicationStatus WHEN 1 THEN 'New' WHEN 2 THEN 'Cancelled' WHEN 3 THEN 'Completed' ELSE 'Unknown' END AS Status,
                                    a.PaidFees
                             FROM Applications a
                             INNER JOIN People p ON p.ID = a.ApplicantPersonID
                             INNER JOIN ApplicationTypes at ON at.ID = a.ApplicationTypeID
                             ORDER BY a.ID DESC";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
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

        public static async Task<int> GetApplicationsCountAsync()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Applications", connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }

        public static async Task<int> GetApplicationsCountByStatusAsync(byte status)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Applications WHERE ApplicationStatus = @Status", connection))
            {
                command.Parameters.Add("@Status", SqlDbType.TinyInt).Value = status;
                await connection.OpenAsync().ConfigureAwait(false);
                return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }

        public static async Task<int> AddAsync(ApplicationData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO Applications (ApplicantPersonID, ApplicationTypeID, CreatedByUserID, ApplicationDate, ApplicationStatus, LastStatusDate, PaidFees, UpdatedAt, CreatedAt, IsActive) VALUES (@ApplicantPersonID, @ApplicationTypeID, @CreatedByUserID, @ApplicationDate, @ApplicationStatus, @LastStatusDate, @PaidFees, @UpdatedAt, @CreatedAt, @IsActive); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = data.ApplicantPersonID;
                command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = data.ApplicationTypeID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@ApplicationDate", SqlDbType.DateTime).Value = data.ApplicationDate;
                command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = data.ApplicationStatus;
                command.Parameters.Add("@LastStatusDate", SqlDbType.DateTime).Value = data.LastStatusDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = data.UpdatedAt;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(ApplicationData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE Applications SET ApplicantPersonID = @ApplicantPersonID, ApplicationTypeID = @ApplicationTypeID, CreatedByUserID = @CreatedByUserID, ApplicationDate = @ApplicationDate, ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate, PaidFees = @PaidFees, UpdatedAt = @UpdatedAt, CreatedAt = @CreatedAt, IsActive = @IsActive WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = data.ApplicantPersonID;
                command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = data.ApplicationTypeID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@ApplicationDate", SqlDbType.DateTime).Value = data.ApplicationDate;
                command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = data.ApplicationStatus;
                command.Parameters.Add("@LastStatusDate", SqlDbType.DateTime).Value = data.LastStatusDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = data.UpdatedAt;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Applications WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM Applications WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
