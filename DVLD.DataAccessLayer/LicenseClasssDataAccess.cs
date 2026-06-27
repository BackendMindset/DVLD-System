using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class LicenseClasssDataAccess
    {
        private static LicenseClassData Map(SqlDataReader reader)
        {
            return new LicenseClassData
            {
                ID = (int)reader["ID"],
                ClassName = reader["ClassName"] == DBNull.Value ? string.Empty : (string)reader["ClassName"],
                ClassDescription = reader["ClassDescription"] == DBNull.Value ? string.Empty : (string)reader["ClassDescription"],
                MinimumAllowedAge = Convert.ToByte(reader["MinimumAllowedAge"]),
                ValidityLength = Convert.ToByte(reader["ValidityLength"]),
                LicenseFee = (decimal)reader["LicenseFee"],
            };
        }

        public static async Task<LicenseClassData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM LicenseClasses WHERE ID = @ID", connection))
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

        public static async Task<List<LicenseClassData>> GetAllAsync()
        {
            List<LicenseClassData> list = new List<LicenseClassData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM LicenseClasses", connection))
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

        public static async Task<int> AddAsync(LicenseClassData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO LicenseClasses (ClassName, ClassDescription, MinimumAllowedAge, ValidityLength, LicenseFee) VALUES (@ClassName, @ClassDescription, @MinimumAllowedAge, @ValidityLength, @LicenseFee); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ClassName", SqlDbType.NVarChar, 100).Value = data.ClassName ?? (object)DBNull.Value;
                command.Parameters.Add("@ClassDescription", SqlDbType.NVarChar, 500).Value = data.ClassDescription ?? (object)DBNull.Value;
                command.Parameters.Add("@MinimumAllowedAge", SqlDbType.TinyInt).Value = data.MinimumAllowedAge;
                command.Parameters.Add("@ValidityLength", SqlDbType.TinyInt).Value = data.ValidityLength;
                command.Parameters.Add("@LicenseFee", SqlDbType.Decimal).Value = data.LicenseFee;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(LicenseClassData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE LicenseClasses SET ClassName = @ClassName, ClassDescription = @ClassDescription, MinimumAllowedAge = @MinimumAllowedAge, ValidityLength = @ValidityLength, LicenseFee = @LicenseFee WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ClassName", SqlDbType.NVarChar, 100).Value = data.ClassName ?? (object)DBNull.Value;
                command.Parameters.Add("@ClassDescription", SqlDbType.NVarChar, 500).Value = data.ClassDescription ?? (object)DBNull.Value;
                command.Parameters.Add("@MinimumAllowedAge", SqlDbType.TinyInt).Value = data.MinimumAllowedAge;
                command.Parameters.Add("@ValidityLength", SqlDbType.TinyInt).Value = data.ValidityLength;
                command.Parameters.Add("@LicenseFee", SqlDbType.Decimal).Value = data.LicenseFee;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM LicenseClasses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM LicenseClasses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}