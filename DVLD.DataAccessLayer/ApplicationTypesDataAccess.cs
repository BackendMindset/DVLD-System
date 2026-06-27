using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class ApplicationTypesDataAccess
    {
        private static ApplicationTypeData Map(SqlDataReader reader)
        {
            return new ApplicationTypeData
            {
                ID = (int)reader["ID"],
                ApplicationTypeTitle = reader["ApplicationTypeTitle"] == DBNull.Value ? string.Empty : (string)reader["ApplicationTypeTitle"],
                ApplicationFees = (decimal)reader["ApplicationFees"],
            };
        }

        public static async Task<ApplicationTypeData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM ApplicationTypes WHERE ID = @ID", connection))
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

        public static async Task<List<ApplicationTypeData>> GetAllAsync()
        {
            List<ApplicationTypeData> list = new List<ApplicationTypeData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM ApplicationTypes", connection))
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

        public static async Task<int> AddAsync(ApplicationTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees) VALUES (@ApplicationTypeTitle, @ApplicationFees); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ApplicationTypeTitle", SqlDbType.NVarChar, 150).Value = data.ApplicationTypeTitle ?? (object)DBNull.Value;
                command.Parameters.Add("@ApplicationFees", SqlDbType.Decimal).Value = data.ApplicationFees;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(ApplicationTypeData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE ApplicationTypes SET ApplicationTypeTitle = @ApplicationTypeTitle, ApplicationFees = @ApplicationFees WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ApplicationTypeTitle", SqlDbType.NVarChar, 150).Value = data.ApplicationTypeTitle ?? (object)DBNull.Value;
                command.Parameters.Add("@ApplicationFees", SqlDbType.Decimal).Value = data.ApplicationFees;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM ApplicationTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM ApplicationTypes WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}