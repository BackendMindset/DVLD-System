using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class ViolationPaymentsDataAccess
    {
        private static ViolationPaymentData Map(SqlDataReader reader)
        {
            return new ViolationPaymentData
            {
                ID = (int)reader["ID"],
                ViolationID = (int)reader["ViolationID"],
                PaymentID = (int)reader["PaymentID"],
                PaidAmount = (decimal)reader["PaidAmount"],
                PaidDate = (DateTime)reader["PaidDate"],
                Status = reader["Status"] == DBNull.Value ? string.Empty : (string)reader["Status"],
            };
        }

        public static async Task<ViolationPaymentData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM ViolationPayments WHERE ID = @ID", connection))
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

        public static async Task<List<ViolationPaymentData>> GetAllAsync()
        {
            List<ViolationPaymentData> list = new List<ViolationPaymentData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM ViolationPayments", connection))
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

        public static async Task<int> AddAsync(ViolationPaymentData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO ViolationPayments (ViolationID, PaymentID, PaidAmount, PaidDate, Status) VALUES (@ViolationID, @PaymentID, @PaidAmount, @PaidDate, @Status); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ViolationID", SqlDbType.Int).Value = data.ViolationID;
                command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = data.PaymentID;
                command.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = data.PaidAmount;
                command.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = data.PaidDate;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = data.Status ?? (object)DBNull.Value;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(ViolationPaymentData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE ViolationPayments SET ViolationID = @ViolationID, PaymentID = @PaymentID, PaidAmount = @PaidAmount, PaidDate = @PaidDate, Status = @Status WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ViolationID", SqlDbType.Int).Value = data.ViolationID;
                command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = data.PaymentID;
                command.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = data.PaidAmount;
                command.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = data.PaidDate;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = data.Status ?? (object)DBNull.Value;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM ViolationPayments WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM ViolationPayments WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}