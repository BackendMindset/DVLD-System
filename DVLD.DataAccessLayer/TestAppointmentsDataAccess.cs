using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class TestAppointmentsDataAccess
    {
        private static TestAppointmentData Map(SqlDataReader reader)
        {
            return new TestAppointmentData
            {
                ID = (int)reader["ID"],
                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
                TestTypeID = (int)reader["TestTypeID"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
                AppointmentDate = (DateTime)reader["AppointmentDate"],
                PaidFees = (decimal)reader["PaidFees"],
                IsLocked = (bool)reader["IsLocked"],
            };
        }

        public static async Task<TestAppointmentData> GetByIDAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM TestAppointments WHERE ID = @ID", connection))
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

        public static async Task<List<TestAppointmentData>> GetAllAsync()
        {
            List<TestAppointmentData> list = new List<TestAppointmentData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM TestAppointments", connection))
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

        public static async Task<List<TestAppointmentData>> GetByLocalDrivingLicenseApplicationIdAsync(int localDrivingLicenseApplicationId)
        {
            List<TestAppointmentData> list = new List<TestAppointmentData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT * FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID ORDER BY ID DESC", connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = localDrivingLicenseApplicationId;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(Map(reader));
                }
            }

            return list;
        }

        public static async Task<bool> HasUnlockedAppointmentAsync(int localDrivingLicenseApplicationId, int testTypeId)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT 1
                                                         FROM TestAppointments
                                                         WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                                           AND TestTypeID = @TestTypeID
                                                           AND IsLocked = 0", connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = localDrivingLicenseApplicationId;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testTypeId;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }

        public static async Task<int> AddAsync(TestAppointmentData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO TestAppointments (LocalDrivingLicenseApplicationID, TestTypeID, CreatedByUserID, AppointmentDate, PaidFees, IsLocked) VALUES (@LocalDrivingLicenseApplicationID, @TestTypeID, @CreatedByUserID, @AppointmentDate, @PaidFees, @IsLocked); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = data.LocalDrivingLicenseApplicationID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = data.TestTypeID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = data.AppointmentDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = data.IsLocked;
                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static async Task<bool> UpdateAsync(TestAppointmentData data)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE TestAppointments SET LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID, TestTypeID = @TestTypeID, CreatedByUserID = @CreatedByUserID, AppointmentDate = @AppointmentDate, PaidFees = @PaidFees, IsLocked = @IsLocked WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = data.LocalDrivingLicenseApplicationID;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = data.TestTypeID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = data.CreatedByUserID;
                command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = data.AppointmentDate;
                command.Parameters.Add("@PaidFees", SqlDbType.Decimal).Value = data.PaidFees;
                command.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = data.IsLocked;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = data.ID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM TestAppointments WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }

        public static async Task<bool> ExistsAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM TestAppointments WHERE ID = @ID", connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
