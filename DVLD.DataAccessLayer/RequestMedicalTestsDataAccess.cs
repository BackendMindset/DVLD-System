using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class RequestMedicalTestsDataAccess
    {
        private static RequestMedicalTestData Map(SqlDataReader reader)
        {
            return new RequestMedicalTestData
            {
                ID = (int)reader["ID"],
                ApplicationID = (int)reader["ApplicationID"],
                CenterID = (int)reader["CenterID"],
                TestTypeID = (int)reader["TestTypeID"],
                Status = reader["Status"] == DBNull.Value ? string.Empty : (string)reader["Status"],
                ExamDate = (DateTime)reader["ExamDate"],
                PaidFees = (decimal)reader["PaidFees"],
            };
        }

        public static async Task<RequestMedicalTestData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM RequestMedicalTests WHERE ID = @ID", connection))
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

        public static async Task<List<RequestMedicalTestData>> GetAllAsync()
        {
            List<RequestMedicalTestData> list = new List<RequestMedicalTestData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM RequestMedicalTests", connection))
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

        public static async Task<int> AddAsync(RequestMedicalTestData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO RequestMedicalTests (ApplicationID, CenterID, TestTypeID, Status, ExamDate, PaidFees) VALUES (@ApplicationID, @CenterID, @TestTypeID, @Status, @ExamDate, @PaidFees); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@CenterID", SqlDbType.Int).Value = data.CenterID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = data.TestTypeID;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = data.Status ?? (object)DBNull.Value;
                command.Parameters.Add("@ExamDate", SqlDbType.DateTime).Value = data.ExamDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(RequestMedicalTestData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE RequestMedicalTests SET ApplicationID = @ApplicationID, CenterID = @CenterID, TestTypeID = @TestTypeID, Status = @Status, ExamDate = @ExamDate, PaidFees = @PaidFees WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@CenterID", SqlDbType.Int).Value = data.CenterID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = data.TestTypeID;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = data.Status ?? (object)DBNull.Value;
                command.Parameters.Add("@ExamDate", SqlDbType.DateTime).Value = data.ExamDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM RequestMedicalTests WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM RequestMedicalTests WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}