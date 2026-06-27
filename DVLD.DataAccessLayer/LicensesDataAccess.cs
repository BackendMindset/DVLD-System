using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class LicensesDataAccess
    {
        private static LicenseData Map(SqlDataReader reader)
        {
            return new LicenseData
            {
                ID = (int)reader["ID"],
                ApplicationID = (int)reader["ApplicationID"],
                DriverID = (int)reader["DriverID"],
                LicenseClassID = (int)reader["LicenseClassID"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
                IssueDate = (DateTime)reader["IssueDate"],
                ExpirationDate = (DateTime)reader["ExpirationDate"],
                PaidFees = (decimal)reader["PaidFees"],
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
                IsActive = (bool)reader["IsActive"],
                IssueReason = Convert.ToByte(reader["IssueReason"]),
                LicenseStatus = reader["LicenseStatus"] == DBNull.Value ? string.Empty : (string)reader["LicenseStatus"],
            };
        }

        private static LicenseListData MapList(SqlDataReader reader)
        {
            return new LicenseListData
            {
                LicenseID = (int)reader["LicenseID"],
                DriverName = reader["DriverName"] as string ?? string.Empty,
                ClassName = reader["ClassName"] as string ?? string.Empty,
                IssueDate = (DateTime)reader["IssueDate"],
                ExpirationDate = (DateTime)reader["ExpirationDate"],
                LicenseStatus = reader["LicenseStatus"] as string ?? string.Empty,
                IsActive = (bool)reader["IsActive"]
            };
        }

        public static async Task<LicenseData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Licenses WHERE ID = @ID", connection))
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

        public static async Task<List<LicenseData>> GetAllAsync()
        {
            List<LicenseData> list = new List<LicenseData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Licenses", connection))
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

        public static async Task<List<LicenseData>> GetByDriverIdAsync(int driverId)
        {
            List<LicenseData> list = new List<LicenseData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Licenses WHERE DriverID = @DriverID ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public static async Task<List<LicenseListData>> SearchLicensesAsync(string value)
        {
            List<LicenseListData> list = new List<LicenseListData>();
            if (string.IsNullOrWhiteSpace(value))
                return list;

            bool isId = int.TryParse(value.Trim(), out int id);
            string query = @"SELECT l.ID AS LicenseID,
                                    p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName AS DriverName,
                                    lc.ClassName,
                                    l.IssueDate,
                                    l.ExpirationDate,
                                    l.LicenseStatus,
                                    l.IsActive
                             FROM Licenses l
                             INNER JOIN Drivers d ON d.ID = l.DriverID
                             INNER JOIN People p ON p.ID = d.PersonID
                             INNER JOIN LicenseClasses lc ON lc.ID = l.LicenseClassID
                             WHERE (@ID IS NOT NULL AND (l.ID = @ID OR l.ApplicationID = @ID OR l.DriverID = @ID))
                                OR p.NationalID LIKE @Value
                                OR (p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName) LIKE @Value
                                OR l.LicenseStatus LIKE @Value
                             ORDER BY l.ID DESC";

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

        public static async Task<List<LicenseListData>> GetLicensesForListAsync()
        {
            List<LicenseListData> list = new List<LicenseListData>();
            string query = @"SELECT l.ID AS LicenseID,
                                    p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName AS DriverName,
                                    lc.ClassName,
                                    l.IssueDate,
                                    l.ExpirationDate,
                                    l.LicenseStatus,
                                    l.IsActive
                             FROM Licenses l
                             INNER JOIN Drivers d ON d.ID = l.DriverID
                             INNER JOIN People p ON p.ID = d.PersonID
                             INNER JOIN LicenseClasses lc ON lc.ID = l.LicenseClassID
                             ORDER BY l.ID DESC";

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

        public static async Task<int> GetActiveLicensesCountAsync()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Licenses WHERE IsActive = 1", connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }

        public static async Task<int> AddAsync(LicenseData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClassID, CreatedByUserID, IssueDate, ExpirationDate, PaidFees, Notes, IsActive, IssueReason, LicenseStatus) VALUES (@ApplicationID, @DriverID, @LicenseClassID, @CreatedByUserID, @IssueDate, @ExpirationDate, @PaidFees, @Notes, @IsActive, @IssueReason, @LicenseStatus); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = data.DriverID;
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = data.LicenseClassID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = data.IssueDate;
                command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = data.ExpirationDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = data.Notes ?? (object)DBNull.Value;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                command.Parameters.Add("@IssueReason", SqlDbType.TinyInt).Value = data.IssueReason;
                command.Parameters.Add("@LicenseStatus", SqlDbType.NVarChar, 20).Value = data.LicenseStatus ?? (object)DBNull.Value;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(LicenseData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE Licenses SET ApplicationID = @ApplicationID, DriverID = @DriverID, LicenseClassID = @LicenseClassID, CreatedByUserID = @CreatedByUserID, IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, PaidFees = @PaidFees, Notes = @Notes, IsActive = @IsActive, IssueReason = @IssueReason, LicenseStatus = @LicenseStatus WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = data.DriverID;
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = data.LicenseClassID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = data.IssueDate;
                command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = data.ExpirationDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = data.Notes ?? (object)DBNull.Value;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                command.Parameters.Add("@IssueReason", SqlDbType.TinyInt).Value = data.IssueReason;
                command.Parameters.Add("@LicenseStatus", SqlDbType.NVarChar, 20).Value = data.LicenseStatus ?? (object)DBNull.Value;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Licenses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM Licenses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
