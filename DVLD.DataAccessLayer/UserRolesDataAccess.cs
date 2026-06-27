using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class UserRolesDataAccess
    {
        private static UserRoleData Map(SqlDataReader reader)
        {
            return new UserRoleData
            {
                ID = (int)reader["ID"],
                UserID = (int)reader["UserID"],
                RoleID = (int)reader["RoleID"],
            };
        }

        public static async Task<UserRoleData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM UserRoles WHERE ID = @ID", connection))
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

        public static async Task<List<UserRoleData>> GetAllAsync()
        {
            List<UserRoleData> list = new List<UserRoleData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM UserRoles", connection))
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

        public static async Task<List<UserRoleData>> GetByUserIdAsync(int userId)
        {
            List<UserRoleData> list = new List<UserRoleData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM UserRoles WHERE UserID = @UserID ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }

            return list;
        }

        public static async Task<int> AddAsync(UserRoleData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO UserRoles (UserID, RoleID) VALUES (@UserID, @RoleID); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = data.UserID;
                command.Parameters.Add("@RoleID", SqlDbType.Int).Value = data.RoleID;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(UserRoleData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE UserRoles SET UserID = @UserID, RoleID = @RoleID WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = data.UserID;
                command.Parameters.Add("@RoleID", SqlDbType.Int).Value = data.RoleID;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM UserRoles WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM UserRoles WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
