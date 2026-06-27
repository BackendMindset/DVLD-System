using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class MedicalCentersDataAccess
    {
        private static MedicalCenterData Map(SqlDataReader reader)
        {
            return new MedicalCenterData
            {
                ID = (int)reader["ID"],
                CenterName = reader["CenterName"] == DBNull.Value ? string.Empty : (string)reader["CenterName"],
                Address = reader["Address"] == DBNull.Value ? string.Empty : (string)reader["Address"],
                Phone = reader["Phone"] == DBNull.Value ? string.Empty : (string)reader["Phone"],
                IsActive = (bool)reader["IsActive"],
            };
        }

        public static async Task<MedicalCenterData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalCenters WHERE ID = @ID", connection))
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

        public static async Task<List<MedicalCenterData>> GetAllAsync()
        {
            List<MedicalCenterData> list = new List<MedicalCenterData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalCenters", connection))
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

        public static async Task<int> AddAsync(MedicalCenterData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO MedicalCenters (CenterName, Address, Phone, IsActive) VALUES (@CenterName, @Address, @Phone, @IsActive); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@CenterName", SqlDbType.NVarChar, 150).Value = data.CenterName ?? (object)DBNull.Value;
                command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = data.Address ?? (object)DBNull.Value;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = data.Phone ?? (object)DBNull.Value;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(MedicalCenterData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE MedicalCenters SET CenterName = @CenterName, Address = @Address, Phone = @Phone, IsActive = @IsActive WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@CenterName", SqlDbType.NVarChar, 150).Value = data.CenterName ?? (object)DBNull.Value;
                command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = data.Address ?? (object)DBNull.Value;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = data.Phone ?? (object)DBNull.Value;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM MedicalCenters WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM MedicalCenters WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}