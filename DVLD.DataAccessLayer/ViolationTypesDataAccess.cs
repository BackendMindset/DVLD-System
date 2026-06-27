using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class ViolationTypesDataAccess
    {
        private static ViolationTypeData Map(SqlDataReader reader)
        {
            return new ViolationTypeData
            {
                ID = (int)reader["ID"],
                ViolationName = reader["ViolationName"] == DBNull.Value ? string.Empty : (string)reader["ViolationName"],
                Description = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"],
                BaseFineAmount = (decimal)reader["BaseFineAmount"],
            };
        }

        public static async Task<ViolationTypeData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM ViolationTypes WHERE ID = @ID", connection))
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

        public static async Task<List<ViolationTypeData>> GetAllAsync()
        {
            List<ViolationTypeData> list = new List<ViolationTypeData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM ViolationTypes", connection))
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

        public static async Task<int> AddAsync(ViolationTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO ViolationTypes (ViolationName, Description, BaseFineAmount) VALUES (@ViolationName, @Description, @BaseFineAmount); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ViolationName", SqlDbType.NVarChar, 100).Value = data.ViolationName ?? (object)DBNull.Value;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = data.Description ?? (object)DBNull.Value;
                command.Parameters.Add("@BaseFineAmount", SqlDbType.Decimal).Value = data.BaseFineAmount;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(ViolationTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE ViolationTypes SET ViolationName = @ViolationName, Description = @Description, BaseFineAmount = @BaseFineAmount WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ViolationName", SqlDbType.NVarChar, 100).Value = data.ViolationName ?? (object)DBNull.Value;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = data.Description ?? (object)DBNull.Value;
                command.Parameters.Add("@BaseFineAmount", SqlDbType.Decimal).Value = data.BaseFineAmount;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM ViolationTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM ViolationTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}