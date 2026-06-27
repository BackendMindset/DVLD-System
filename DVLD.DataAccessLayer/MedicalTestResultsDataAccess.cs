using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class MedicalTestResultsDataAccess
    {
        private static MedicalTestResultData Map(SqlDataReader reader)
        {
            return new MedicalTestResultData
            {
                ID = (int)reader["ID"],
                RequestMedicalTestID = (int)reader["RequestMedicalTestID"],
                Result = (bool)reader["Result"],
                ResultDetails = reader["ResultDetails"] == DBNull.Value ? string.Empty : (string)reader["ResultDetails"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
            };
        }

        public static async Task<MedicalTestResultData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalTestResults WHERE ID = @ID", connection))
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

        public static async Task<List<MedicalTestResultData>> GetAllAsync()
        {
            List<MedicalTestResultData> list = new List<MedicalTestResultData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalTestResults", connection))
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

        public static async Task<int> AddAsync(MedicalTestResultData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO MedicalTestResults (RequestMedicalTestID, Result, ResultDetails, CreatedAt, CreatedByUserID) VALUES (@RequestMedicalTestID, @Result, @ResultDetails, @CreatedAt, @CreatedByUserID); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@RequestMedicalTestID", SqlDbType.Int).Value = data.RequestMedicalTestID;
                command.Parameters.Add("@Result", SqlDbType.Bit).Value = data.Result;
                command.Parameters.Add("@ResultDetails", SqlDbType.NVarChar, 1000).Value = data.ResultDetails ?? (object)DBNull.Value;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(MedicalTestResultData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE MedicalTestResults SET RequestMedicalTestID = @RequestMedicalTestID, Result = @Result, ResultDetails = @ResultDetails, CreatedAt = @CreatedAt, CreatedByUserID = @CreatedByUserID WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@RequestMedicalTestID", SqlDbType.Int).Value = data.RequestMedicalTestID;
                command.Parameters.Add("@Result", SqlDbType.Bit).Value = data.Result;
                command.Parameters.Add("@ResultDetails", SqlDbType.NVarChar, 1000).Value = data.ResultDetails ?? (object)DBNull.Value;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM MedicalTestResults WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM MedicalTestResults WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}