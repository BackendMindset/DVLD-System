using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccess
{
    public static class UsersDataAccess
    {
        private static UserData Map(SqlDataReader reader)
        {
            return new UserData
            {
                ID = (int)reader["ID"],
                PersonID = (int)reader["PersonID"],
                UserName = (string)reader["UserName"],
                PasswordHash = (string)reader["PasswordHash"],
                IsActive = (bool)reader["IsActive"],
                LastLogin = reader["LastLogin"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["LastLogin"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }
        private static UserListData MapToList(SqlDataReader reader)
        {
            return new UserListData
            {
                UserID = (int)reader["UserID"],
                PersonID = (int)reader["PersonID"],
                FullName = reader["FullName"] as string ?? string.Empty,
                UserName = reader["UserName"] as string ?? string.Empty,
                IsActive = (bool)reader["IsActive"]
            };
        }

        public static async Task<bool> ExistsByPersonIDAsync(int personId)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM Users WHERE PersonID = @PersonID", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }

        public static async Task<List<UserListData>> SearchUsersAsync(string value)
        {
            List<UserListData> list = new List<UserListData>();
            if (string.IsNullOrWhiteSpace(value))
                return list;

            bool isId = int.TryParse(value.Trim(), out int id);
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"SELECT UserID, PersonID, FullName, UserName, IsActive
                  FROM Users_View
                  WHERE (@ID IS NOT NULL AND (UserID = @ID OR PersonID = @ID))
                     OR UserName LIKE @Value
                     OR FullName LIKE @Value
                  ORDER BY UserID DESC", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = isId ? (object)id : DBNull.Value;
                command.Parameters.Add("@Value", SqlDbType.NVarChar, 150).Value = "%" + value.Trim() + "%";
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(MapToList(reader));
                }
            }

            return list;
        }
        public static async Task<UserData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT ID, PersonID, UserName, PasswordHash, IsActive, LastLogin, CreatedAt FROM Users WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                await connection.OpenAsync().ConfigureAwait(false);

                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        return Map(reader);
                    }
                    return null;
                }
            }
        }
        public static async Task<UserData> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT ID, PersonID, UserName, PasswordHash, IsActive, LastLogin, CreatedAt FROM Users WHERE UserName = @UserName", connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName.Trim();
                await connection.OpenAsync().ConfigureAwait(false);

                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        return Map(reader);
                    }
                    return null;
                }
            }
        }
        public static async Task<List<UserListData>> GetUsersForListAsync()
        {
            List<UserListData> list = new List<UserListData>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT UserID, PersonID, FullName, UserName, IsActive FROM Users_View", connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        list.Add(MapToList(reader));
                    }
                }
            }
            return list;
        }
        public static async Task<int> AddAsync(UserData user)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"INSERT INTO Users (PersonID, UserName, PasswordHash, IsActive)
                  VALUES (@PersonID, @UserName, @PasswordHash, @IsActive);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = user.PersonID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = user.UserName ?? (object)DBNull.Value;
                command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = user.PasswordHash;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;

                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        public static async Task<bool> UpdateAsync(UserData user)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"UPDATE Users SET 
                  PersonID = @PersonID,
                  UserName = @UserName,
                  IsActive = @IsActive
                  WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = user.ID;
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = user.PersonID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = user.UserName;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;
                await connection.OpenAsync().ConfigureAwait(false);
                int affectedRows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return affectedRows > 0;
            }
        }
        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "DELETE FROM Users WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                await connection.OpenAsync().ConfigureAwait(false);

                int affectedRows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                return affectedRows > 0;
            }
        }
        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT 1 FROM Users WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                await connection.OpenAsync().ConfigureAwait(false);

                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);

                return result != null;
            }
        }
        public static async Task<bool> UserNameExistsAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT 1 FROM Users WHERE UserName = @UserName", connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName.Trim();

                await connection.OpenAsync().ConfigureAwait(false);

                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);

                return result != null;
            }
        }

        public static async Task<bool> UpdatePasswordAsync(int id, string passwordHash)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "UPDATE Users SET PasswordHash = @PasswordHash WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = passwordHash;

                await connection.OpenAsync().ConfigureAwait(false);

                int affectedRows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                return affectedRows > 0;
            }
        }

        public static async Task<bool> SetActiveAsync(int id, bool isActive)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(
                "UPDATE Users SET IsActive = @IsActive WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

                await connection.OpenAsync().ConfigureAwait(false);

                int affectedRows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                return affectedRows > 0;
            }
        }
        //public static async Task<bool> UpdateLastLoginAsync(int id)
        //{
        //    using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
        //    using (SqlCommand command = new SqlCommand(
        //        "UPDATE Users SET LastLogin = GETDATE() WHERE ID = @ID", connection))
        //    {
        //        command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

        //        await connection.OpenAsync().ConfigureAwait(false);

        //        int affectedRows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

        //        return affectedRows > 0;
        //    }
        //}
        public static async Task<bool> UpdateLastLoginAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("UpdateUserLastLogin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                int rows = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return rows > 0;
            }
        }
    }
}
