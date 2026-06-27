using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class MedicalCenterTestsDataAccess
    {
        private static MedicalCenterTestData Map(SqlDataReader reader)
        {
            return new MedicalCenterTestData
            {
                ID = (int)reader["ID"],
                CenterID = (int)reader["CenterID"],
                TestTypeID = (int)reader["TestTypeID"],
                Fee = (decimal)reader["Fee"],
            };
        }

        public static async Task<MedicalCenterTestData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalCenterTests WHERE ID = @ID", connection))
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

        public static async Task<List<MedicalCenterTestData>> GetAllAsync()
        {
            List<MedicalCenterTestData> list = new List<MedicalCenterTestData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM MedicalCenterTests", connection))
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

        public static async Task<int> AddAsync(MedicalCenterTestData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO MedicalCenterTests (CenterID, TestTypeID, Fee) VALUES (@CenterID, @TestTypeID, @Fee); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@CenterID", SqlDbType.Int).Value = data.CenterID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = data.TestTypeID;
                command.Parameters.Add("@Fee", SqlDbType.Decimal).Value = data.Fee;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(MedicalCenterTestData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE MedicalCenterTests SET CenterID = @CenterID, TestTypeID = @TestTypeID, Fee = @Fee WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@CenterID", SqlDbType.Int).Value = data.CenterID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = data.TestTypeID;
                command.Parameters.Add("@Fee", SqlDbType.Decimal).Value = data.Fee;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM MedicalCenterTests WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM MedicalCenterTests WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}