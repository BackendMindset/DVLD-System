using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class PaymentsDataAccess
    {
        private static PaymentData Map(SqlDataReader reader)
        {
            return new PaymentData
            {
                ID = (int)reader["ID"],
                ApplicationID = (int)reader["ApplicationID"],
                Amount = (decimal)reader["Amount"],
                PaymentMethod = reader["PaymentMethod"] == DBNull.Value ? string.Empty : (string)reader["PaymentMethod"],
                Status = reader["Status"] == DBNull.Value ? string.Empty : (string)reader["Status"],
                ReferenceNumber = reader["ReferenceNumber"] == DBNull.Value ? string.Empty : (string)reader["ReferenceNumber"],
                PaymentDate = (DateTime)reader["PaymentDate"],
            };
        }

        public static async Task<PaymentData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Payments WHERE ID = @ID", connection))
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

        public static async Task<List<PaymentData>> GetAllAsync()
        {
            List<PaymentData> list = new List<PaymentData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Payments", connection))
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

        public static async Task<List<PaymentData>> GetByApplicationIdAsync(int applicationId)
        {
            List<PaymentData> list = new List<PaymentData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM Payments WHERE ApplicationID = @ApplicationID ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }

            return list;
        }

        public static async Task<int> AddAsync(PaymentData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO Payments (ApplicationID, Amount, PaymentMethod, Status, ReferenceNumber, PaymentDate) VALUES (@ApplicationID, @Amount, @PaymentMethod, @Status, @ReferenceNumber, @PaymentDate); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = data.Amount;
                command.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 20).Value = data.PaymentMethod ?? (object)DBNull.Value;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = data.Status ?? (object)DBNull.Value;
                command.Parameters.Add("@ReferenceNumber", SqlDbType.NVarChar, 100).Value = data.ReferenceNumber ?? (object)DBNull.Value;
                command.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = data.PaymentDate;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(PaymentData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE Payments SET ApplicationID = @ApplicationID, Amount = @Amount, PaymentMethod = @PaymentMethod, Status = @Status, ReferenceNumber = @ReferenceNumber, PaymentDate = @PaymentDate WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = data.ApplicationID;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = data.Amount;
                command.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 20).Value = data.PaymentMethod ?? (object)DBNull.Value;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = data.Status ?? (object)DBNull.Value;
                command.Parameters.Add("@ReferenceNumber", SqlDbType.NVarChar, 100).Value = data.ReferenceNumber ?? (object)DBNull.Value;
                command.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = data.PaymentDate;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM Payments WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM Payments WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
