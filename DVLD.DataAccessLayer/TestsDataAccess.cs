using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class TestsDataAccess
    {
        private static TestData Map(SqlDataReader reader)
        {
            return new TestData
            {
                ID = (int)reader["ID"],
                TestAppointmentID = (int)reader["TestAppointmentID"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                TestResult = Convert.ToByte(reader["TestResult"]),
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : (string)reader["Notes"],
            };
        }

        public static async Task<TestData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Tests WHERE ID = @ID", connection))
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

        public static async Task<List<TestData>> GetAllAsync()
        {
            List<TestData> list = new List<TestData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Tests", connection))
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

        public static async Task<List<TestData>> GetByApplicationIdAsync(int applicationId)
        {
            List<TestData> list = new List<TestData>();
            string query = @"SELECT t.*
                             FROM Tests t
                             INNER JOIN TestAppointments ta ON ta.ID = t.TestAppointmentID
                             INNER JOIN LocalDrivingLicenseApplications ldla ON ldla.ID = ta.LocalDrivingLicenseApplicationID
                             WHERE ldla.ApplicationID = @ApplicationID
                             ORDER BY t.ID DESC";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }

            return list;
        }

        public static async Task<int> AddAsync(TestData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO Tests (TestAppointmentID, CreatedByUserID, CreatedAt, TestResult, Notes) VALUES (@TestAppointmentID, @CreatedByUserID, @CreatedAt, @TestResult, @Notes); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = data.TestAppointmentID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@TestResult", SqlDbType.TinyInt).Value = data.TestResult;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = data.Notes ?? (object)DBNull.Value;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(TestData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE Tests SET TestAppointmentID = @TestAppointmentID, CreatedByUserID = @CreatedByUserID, CreatedAt = @CreatedAt, TestResult = @TestResult, Notes = @Notes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = data.TestAppointmentID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@TestResult", SqlDbType.TinyInt).Value = data.TestResult;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = data.Notes ?? (object)DBNull.Value;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Tests WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM Tests WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
