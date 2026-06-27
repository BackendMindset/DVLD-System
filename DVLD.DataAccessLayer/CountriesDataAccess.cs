using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
        public static class CountriesDataAccess
        {
            public static async Task<CountryData> GetCountryInfoByNameAsync(string countryName)
            {
                CountryData country = new CountryData();
                if (string.IsNullOrWhiteSpace(countryName))
                    return country;
                const string sql = @"SELECT ID, CountryName FROM Countries WHERE CountryName = @CountryName;";
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar, 100).Value = countryName.Trim();
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        country.IsFound = true;
                        country.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                        country.CountryName = reader.GetString(reader.GetOrdinal("CountryName"));
                    }
                    }
                }
                return country;
            }
        public static async Task<CountryData> GetCountryInfoByIDAsync(int ID)
        {
            CountryData country = new CountryData();
            if (ID <= 0)
                return country;
            const string sql = @"SELECT ID, CountryName FROM Countries WHERE ID = @ID;";
            using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                await conn.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        country.IsFound = true;
                        country.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                        country.CountryName = reader.GetString(reader.GetOrdinal("CountryName"));
                    }
                }
            }
            return country;
        }
        public static async Task<List<CountryData>> GetAllCountriesAsync()
            {
                List<CountryData> countries = new List<CountryData>();
                const string sql = "SELECT ID, CountryName FROM Countries ORDER BY CountryName;";
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            CountryData country = new CountryData();
                        country.IsFound = true;
                        country.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                        country.CountryName = reader.GetString(reader.GetOrdinal("CountryName"));
                        countries.Add(country);
                        }
                    }
                }
                return countries;
             }
        }
}

