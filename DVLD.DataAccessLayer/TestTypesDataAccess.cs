using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class TestTypesDataAccess
    {
        private static TestTypeData Map(SqlDataReader reader)
        {
            return new TestTypeData
            {
                ID = (int)reader["ID"],
                TestTypeTitle = reader["TestTypeTitle"] == DBNull.Value ? string.Empty : (string)reader["TestTypeTitle"],
                TestTypeDescription = reader["TestTypeDescription"] == DBNull.Value ? string.Empty : (string)reader["TestTypeDescription"],
                TestTypeFees = (decimal)reader["TestTypeFees"],
                IsRequired = (bool)reader["IsRequired"],
            };
        }

        public static async Task<TestTypeData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM TestTypes WHERE ID = @ID", connection))
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

        public static async Task<List<TestTypeData>> GetAllAsync()
        {
            List<TestTypeData> list = new List<TestTypeData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM TestTypes", connection))
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

        public static async Task<int> AddAsync(TestTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees, IsRequired) VALUES (@TestTypeTitle, @TestTypeDescription, @TestTypeFees, @IsRequired); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@TestTypeTitle", SqlDbType.NVarChar, 100).Value = data.TestTypeTitle ?? (object)DBNull.Value;
                command.Parameters.Add("@TestTypeDescription", SqlDbType.NVarChar, 500).Value = data.TestTypeDescription ?? (object)DBNull.Value;
                command.Parameters.Add("@TestTypeFees", SqlDbType.Decimal).Value = data.TestTypeFees;
                command.Parameters.Add("@IsRequired", SqlDbType.Bit).Value = data.IsRequired;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(TestTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE TestTypes SET TestTypeTitle = @TestTypeTitle, TestTypeDescription = @TestTypeDescription, TestTypeFees = @TestTypeFees, IsRequired = @IsRequired WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@TestTypeTitle", SqlDbType.NVarChar, 100).Value = data.TestTypeTitle ?? (object)DBNull.Value;
                command.Parameters.Add("@TestTypeDescription", SqlDbType.NVarChar, 500).Value = data.TestTypeDescription ?? (object)DBNull.Value;
                command.Parameters.Add("@TestTypeFees", SqlDbType.Decimal).Value = data.TestTypeFees;
                command.Parameters.Add("@IsRequired", SqlDbType.Bit).Value = data.IsRequired;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM TestTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM TestTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}