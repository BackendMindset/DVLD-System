using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class MedicalTestTypesDataAccess
    {
        private static MedicalTestTypeData Map(SqlDataReader reader)
        {
            return new MedicalTestTypeData
            {
                ID = (int)reader["ID"],
                TestName = reader["TestName"] == DBNull.Value ? string.Empty : (string)reader["TestName"],
                Description = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"],
                DefaultFee = (decimal)reader["DefaultFee"],
                IsRequired = (bool)reader["IsRequired"],
            };
        }

        public static async Task<MedicalTestTypeData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalTestTypes WHERE ID = @ID", connection))
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

        public static async Task<List<MedicalTestTypeData>> GetAllAsync()
        {
            List<MedicalTestTypeData> list = new List<MedicalTestTypeData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalTestTypes", connection))
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

        public static async Task<int> AddAsync(MedicalTestTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO MedicalTestTypes (TestName, Description, DefaultFee, IsRequired) VALUES (@TestName, @Description, @DefaultFee, @IsRequired); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@TestName", SqlDbType.NVarChar, 150).Value = data.TestName ?? (object)DBNull.Value;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = data.Description ?? (object)DBNull.Value;
                command.Parameters.Add("@DefaultFee", SqlDbType.Decimal).Value = data.DefaultFee;
                command.Parameters.Add("@IsRequired", SqlDbType.Bit).Value = data.IsRequired;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(MedicalTestTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE MedicalTestTypes SET TestName = @TestName, Description = @Description, DefaultFee = @DefaultFee, IsRequired = @IsRequired WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@TestName", SqlDbType.NVarChar, 150).Value = data.TestName ?? (object)DBNull.Value;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = data.Description ?? (object)DBNull.Value;
                command.Parameters.Add("@DefaultFee", SqlDbType.Decimal).Value = data.DefaultFee;
                command.Parameters.Add("@IsRequired", SqlDbType.Bit).Value = data.IsRequired;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM MedicalTestTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM MedicalTestTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}