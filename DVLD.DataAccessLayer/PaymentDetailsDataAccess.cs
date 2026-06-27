using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class PaymentDetailsDataAccess
    {
        private static PaymentDetailData Map(SqlDataReader reader)
        {
            return new PaymentDetailData
            {
                ID = (int)reader["ID"],
                PaymentID = (int)reader["PaymentID"],
                Amount = (decimal)reader["Amount"],
                ItemName = reader["ItemName"] == DBNull.Value ? string.Empty : (string)reader["ItemName"],
            };
        }

        public static async Task<PaymentDetailData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM PaymentDetails WHERE ID = @ID", connection))
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

        public static async Task<List<PaymentDetailData>> GetAllAsync()
        {
            List<PaymentDetailData> list = new List<PaymentDetailData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM PaymentDetails", connection))
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

        public static async Task<int> AddAsync(PaymentDetailData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO PaymentDetails (PaymentID, Amount, ItemName) VALUES (@PaymentID, @Amount, @ItemName); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = data.PaymentID;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = data.Amount;
                command.Parameters.Add("@ItemName", SqlDbType.NVarChar, 100).Value = data.ItemName ?? (object)DBNull.Value;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(PaymentDetailData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE PaymentDetails SET PaymentID = @PaymentID, Amount = @Amount, ItemName = @ItemName WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = data.PaymentID;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = data.Amount;
                command.Parameters.Add("@ItemName", SqlDbType.NVarChar, 100).Value = data.ItemName ?? (object)DBNull.Value;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM PaymentDetails WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM PaymentDetails WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}