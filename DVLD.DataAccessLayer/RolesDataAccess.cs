using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class RolesDataAccess
    {
        private static RoleData Map(SqlDataReader reader)
        {
            return new RoleData
            {
                ID = (int)reader["ID"],
                RoleName = reader["RoleName"] == DBNull.Value ? string.Empty : (string)reader["RoleName"],
                Description = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"],
                IsActive = (bool)reader["IsActive"],
            };
        }

        public static async Task<RoleData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Roles WHERE ID = @ID", connection))
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

        public static async Task<List<RoleData>> GetAllAsync()
        {
            List<RoleData> list = new List<RoleData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Roles", connection))
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

        public static async Task<int> AddAsync(RoleData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO Roles (RoleName, Description, IsActive) VALUES (@RoleName, @Description, @IsActive); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 150).Value = data.RoleName ?? (object)DBNull.Value;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = data.Description ?? (object)DBNull.Value;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(RoleData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE Roles SET RoleName = @RoleName, Description = @Description, IsActive = @IsActive WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 150).Value = data.RoleName ?? (object)DBNull.Value;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = data.Description ?? (object)DBNull.Value;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Roles WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM Roles WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}