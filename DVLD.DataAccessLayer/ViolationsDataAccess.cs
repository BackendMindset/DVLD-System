using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class ViolationsDataAccess
    {
        private static ViolationData Map(SqlDataReader reader)
        {
            return new ViolationData
            {
                ID = (int)reader["ID"],
                ViolationTypeID = (int)reader["ViolationTypeID"],
                PersonID = (int)reader["PersonID"],
                LicenseID = (int)reader["LicenseID"],
                Location = reader["Location"] == DBNull.Value ? string.Empty : (string)reader["Location"],
                IssueDate = (DateTime)reader["IssueDate"],
                FineAmount = (decimal)reader["FineAmount"],
                Status = reader["Status"] == DBNull.Value ? string.Empty : (string)reader["Status"],
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
            };
        }

        private static ViolationListData MapList(SqlDataReader reader)
        {
            return new ViolationListData
            {
                ViolationID = (int)reader["ViolationID"],
                ViolationType = reader["ViolationType"] as string ?? string.Empty,
                PersonName = reader["PersonName"] as string ?? string.Empty,
                IssueDate = (DateTime)reader["IssueDate"],
                FineAmount = (decimal)reader["FineAmount"],
                Status = reader["Status"] as string ?? string.Empty
            };
        }

        public static async Task<ViolationData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Violations WHERE ID = @ID", connection))
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

        public static async Task<List<ViolationData>> GetAllAsync()
        {
            List<ViolationData> list = new List<ViolationData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Violations", connection))
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

        public static async Task<List<ViolationData>> GetByDriverIdAsync(int driverId)
        {
            List<ViolationData> list = new List<ViolationData>();
            string query = @"SELECT v.*
                             FROM Violations v
                             INNER JOIN Licenses l ON l.ID = v.LicenseID
                             WHERE l.DriverID = @DriverID
                             ORDER BY v.ID DESC";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
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

        public static async Task<List<ViolationListData>> SearchViolationsAsync(string value)
        {
            List<ViolationListData> list = new List<ViolationListData>();
            if (string.IsNullOrWhiteSpace(value))
                return list;

            bool isId = int.TryParse(value.Trim(), out int id);
            string query = @"SELECT v.ID AS ViolationID,
                                    vt.ViolationName AS ViolationType,
                                    p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName AS PersonName,
                                    v.IssueDate,
                                    v.FineAmount,
                                    v.Status
                             FROM Violations v
                             INNER JOIN ViolationTypes vt ON vt.ID = v.ViolationTypeID
                             INNER JOIN People p ON p.ID = v.PersonID
                             WHERE (@ID IS NOT NULL AND (v.ID = @ID OR v.PersonID = @ID OR v.LicenseID = @ID))
                                OR p.NationalID LIKE @Value
                                OR (p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName) LIKE @Value
                                OR vt.ViolationName LIKE @Value
                                OR v.Status LIKE @Value
                             ORDER BY v.ID DESC";

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

        public static async Task<List<ViolationListData>> GetViolationsForListAsync()
        {
            List<ViolationListData> list = new List<ViolationListData>();
            string query = @"SELECT v.ID AS ViolationID,
                                    vt.ViolationName AS ViolationType,
                                    p.FirstName + ' ' + p.SecondName + ' ' + ISNULL(p.ThirdName,'') + ' ' + p.LastName AS PersonName,
                                    v.IssueDate,
                                    v.FineAmount,
                                    v.Status
                             FROM Violations v
                             INNER JOIN ViolationTypes vt ON vt.ID = v.ViolationTypeID
                             INNER JOIN People p ON p.ID = v.PersonID
                             ORDER BY v.ID DESC";

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

        public static async Task<int> AddAsync(ViolationData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO Violations (ViolationTypeID, PersonID, LicenseID, Location, IssueDate, FineAmount, Status, Notes) VALUES (@ViolationTypeID, @PersonID, @LicenseID, @Location, @IssueDate, @FineAmount, @Status, @Notes); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ViolationTypeID", SqlDbType.Int).Value = data.ViolationTypeID;
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = data.PersonID;
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = data.LicenseID;
                command.Parameters.Add("@Location", SqlDbType.NVarChar, 100).Value = data.Location ?? (object)DBNull.Value;
                command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = data.IssueDate;
                command.Parameters.Add("@FineAmount", SqlDbType.Decimal).Value = data.FineAmount;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = data.Status ?? (object)DBNull.Value;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = data.Notes ?? (object)DBNull.Value;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(ViolationData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE Violations SET ViolationTypeID = @ViolationTypeID, PersonID = @PersonID, LicenseID = @LicenseID, Location = @Location, IssueDate = @IssueDate, FineAmount = @FineAmount, Status = @Status, Notes = @Notes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ViolationTypeID", SqlDbType.Int).Value = data.ViolationTypeID;
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = data.PersonID;
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = data.LicenseID;
                command.Parameters.Add("@Location", SqlDbType.NVarChar, 100).Value = data.Location ?? (object)DBNull.Value;
                command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = data.IssueDate;
                command.Parameters.Add("@FineAmount", SqlDbType.Decimal).Value = data.FineAmount;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = data.Status ?? (object)DBNull.Value;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = data.Notes ?? (object)DBNull.Value;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Violations WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM Violations WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
