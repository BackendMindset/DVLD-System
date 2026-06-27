using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class InternationalLicensesDataAccess
    {
        private static InternationalLicenseData Map(SqlDataReader reader)
        {
            return new InternationalLicenseData
            {
                ID = (int)reader["ID"],
                ApplicationID = (int)reader["ApplicationID"],
                DriverID = (int)reader["DriverID"],
                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
                IssuingCountryID = (int)reader["IssuingCountryID"],
                IssueDate = (DateTime)reader["IssueDate"],
                ExpirationDate = (DateTime)reader["ExpirationDate"],
                PaidFees = (decimal)reader["PaidFees"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                IsActive = (bool)reader["IsActive"],
            };
        }

        public static async Task<InternationalLicenseData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM InternationalLicenses WHERE ID = @ID", connection))
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

        public static async Task<List<InternationalLicenseData>> GetAllAsync()
        {
            List<InternationalLicenseData> list = new List<InternationalLicenseData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM InternationalLicenses", connection))
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

        public static async Task<List<InternationalLicenseData>> GetByDriverIdAsync(int driverId)
        {
            List<InternationalLicenseData> list = new List<InternationalLicenseData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM InternationalLicenses WHERE DriverID = @DriverID ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }

            return list;
        }

        public static async Task<bool> HasActiveLicenseForDriverAsync(int driverId)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM InternationalLicenses WHERE DriverID = @DriverID AND IsActive = 1", connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }

        public static async Task<int> AddAsync(InternationalLicenseData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID, CreatedByUserID, IssuingCountryID, IssueDate, ExpirationDate, PaidFees, CreatedAt, IsActive) VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @CreatedByUserID, @IssuingCountryID, @IssueDate, @ExpirationDate, @PaidFees, @CreatedAt, @IsActive); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = data.DriverID;
                command.Parameters.Add("@IssuedUsingLocalLicenseID", SqlDbType.Int).Value = data.IssuedUsingLocalLicenseID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@IssuingCountryID", SqlDbType.Int).Value = data.IssuingCountryID;
                command.Parameters.Add("@IssueDate", SqlDbType.SmallDateTime).Value = data.IssueDate;
                command.Parameters.Add("@ExpirationDate", SqlDbType.SmallDateTime).Value = data.ExpirationDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(InternationalLicenseData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE InternationalLicenses SET ApplicationID = @ApplicationID, DriverID = @DriverID, IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID, CreatedByUserID = @CreatedByUserID, IssuingCountryID = @IssuingCountryID, IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, PaidFees = @PaidFees, CreatedAt = @CreatedAt, IsActive = @IsActive WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = data.DriverID;
                command.Parameters.Add("@IssuedUsingLocalLicenseID", SqlDbType.Int).Value = data.IssuedUsingLocalLicenseID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@IssuingCountryID", SqlDbType.Int).Value = data.IssuingCountryID;
                command.Parameters.Add("@IssueDate", SqlDbType.SmallDateTime).Value = data.IssueDate;
                command.Parameters.Add("@ExpirationDate", SqlDbType.SmallDateTime).Value = data.ExpirationDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = data.CreatedAt;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = data.IsActive;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM InternationalLicenses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM InternationalLicenses WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
