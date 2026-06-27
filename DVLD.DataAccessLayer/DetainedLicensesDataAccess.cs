using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class DetainedLicensesDataAccess
    {
        private static DetainedLicenseData Map(SqlDataReader reader)
        {
            return new DetainedLicenseData
            {
                ID = (int)reader["ID"],
                LicenseID = (int)reader["LicenseID"],
                FineFees = (decimal)reader["FineFees"],
                DetainDate = (DateTime)reader["DetainDate"],
                IsReleased = (bool)reader["IsReleased"],
                ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["ReleaseDate"],
                ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? (int?)null : (int)reader["ReleaseApplicationID"],
                ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? (int?)null : (int)reader["ReleasedByUserID"],
            };
        }

        public static async Task<DetainedLicenseData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM DetainedLicenses WHERE ID = @ID", connection))
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

        public static async Task<List<DetainedLicenseData>> GetAllAsync()
        {
            List<DetainedLicenseData> list = new List<DetainedLicenseData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM DetainedLicenses", connection))
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

        public static async Task<DetainedLicenseData> GetOpenByLicenseIdAsync(int licenseId)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0 ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = licenseId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                        return Map(reader);
                    return null;
                }
            }
        }

        public static async Task<int> GetOpenDetainedLicensesCountAsync()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM DetainedLicenses WHERE IsReleased = 0", connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }

        public static async Task<int> AddAsync(DetainedLicenseData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO DetainedLicenses (LicenseID, FineFees, DetainDate, IsReleased, ReleaseDate, ReleaseApplicationID, ReleasedByUserID) VALUES (@LicenseID, @FineFees, @DetainDate, @IsReleased, @ReleaseDate, @ReleaseApplicationID, @ReleasedByUserID); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = data.LicenseID;
                command.Parameters.Add("@FineFees", SqlDbType.Decimal).Value = data.FineFees;
                command.Parameters.Add("@DetainDate", SqlDbType.SmallDateTime).Value = data.DetainDate;
                command.Parameters.Add("@IsReleased", SqlDbType.Bit).Value = data.IsReleased;
                command.Parameters.Add("@ReleaseDate", SqlDbType.SmallDateTime).Value = data.ReleaseDate;
                command.Parameters.Add("@ReleaseApplicationID", SqlDbType.Int).Value = data.ReleaseApplicationID;
                command.Parameters.Add("@ReleasedByUserID", SqlDbType.Int).Value = data.ReleasedByUserID;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(DetainedLicenseData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE DetainedLicenses SET LicenseID = @LicenseID, FineFees = @FineFees, DetainDate = @DetainDate, IsReleased = @IsReleased, ReleaseDate = @ReleaseDate, ReleaseApplicationID = @ReleaseApplicationID, ReleasedByUserID = @ReleasedByUserID WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = data.LicenseID;
                command.Parameters.Add("@FineFees", SqlDbType.Decimal).Value = data.FineFees;
                command.Parameters.Add("@DetainDate", SqlDbType.SmallDateTime).Value = data.DetainDate;
                command.Parameters.Add("@IsReleased", SqlDbType.Bit).Value = data.IsReleased;
                command.Parameters.Add("@ReleaseDate", SqlDbType.SmallDateTime).Value = data.ReleaseDate;
                command.Parameters.Add("@ReleaseApplicationID", SqlDbType.Int).Value = data.ReleaseApplicationID;
                command.Parameters.Add("@ReleasedByUserID", SqlDbType.Int).Value = data.ReleasedByUserID;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM DetainedLicenses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM DetainedLicenses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
