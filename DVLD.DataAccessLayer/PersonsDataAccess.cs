using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace DVLD.DataAccessLayer
{
    public static class PersonsDataAccess
    {
        private static void MapReaderToPerson(SqlDataReader reader, PersonData person)
        {
            person.IsFound = true;
            person.PersonID = reader.GetInt32(reader.GetOrdinal("ID"));
            person.NationalID = reader["NationalID"].ToString();
            person.FirstName = reader["FirstName"].ToString();
            person.SecondName = reader["SecondName"].ToString();
            person.ThirdName = reader["ThirdName"] == DBNull.Value ? "" : reader["ThirdName"].ToString();
            person.LastName = reader["LastName"].ToString();
            person.Gender = Convert.ToByte(reader["Gender"]);
            person.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
            person.CountryID = Convert.ToInt32(reader["CountryID"]);
            person.Address = reader["Address"].ToString();
            person.Phone = reader["Phone"].ToString();
            person.Email = reader["Email"].ToString();
            person.ImagePath = reader["ImagePath"] == DBNull.Value ? "" : reader["ImagePath"].ToString();
            person.CountryName = reader["CountryName"] == DBNull.Value ? "" : reader["CountryName"].ToString();
        }
        public static async Task<List<PersonData>> SearchAsync(string filterBy, string value)
        {
            List<PersonData> persons = new List<PersonData>();

            if (string.IsNullOrWhiteSpace(value))
                return persons;

            string query = "";

            switch (filterBy)
            {
                case "FirstName":
                    query = "SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, People.CountryID, People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName FROM People LEFT JOIN Countries ON Countries.ID = People.CountryID WHERE People.FirstName LIKE @Value";
                    break;

                case "LastName":
                    query = "SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, People.CountryID, People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName FROM People LEFT JOIN Countries ON Countries.ID = People.CountryID WHERE People.LastName LIKE @Value";
                    break;

                case "Phone":
                    query = "SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, People.CountryID, People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName FROM People LEFT JOIN Countries ON Countries.ID = People.CountryID WHERE People.Phone LIKE @Value";
                    break;

                case "Email":
                    query = "SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, People.CountryID, People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName FROM People LEFT JOIN Countries ON Countries.ID = People.CountryID WHERE People.Email LIKE @Value";
                    break;

                default:
                    return persons;
            }

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Value", "%" + value.Trim() + "%");

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        PersonData person = new PersonData();
                        MapReaderToPerson(reader, person);
                        persons.Add(person);
                    }
                }
            }

            return persons;
        }
        public static async Task<PersonData> GetPersonByNationalIDAsync(string nationalID)
        {
            PersonData person = new PersonData();
            if (string.IsNullOrWhiteSpace(nationalID))
                return person;
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, People.ThirdName, People.LastName, People.Gender,
                                                         People.DateOfBirth, People.CountryID, People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName
                                                         FROM People LEFT JOIN Countries ON Countries.ID = People.CountryID
                                                         WHERE People.NationalID = @NationalID", connection))
            {
                command.Parameters.Add("@NationalID", SqlDbType.NVarChar, 20).Value = nationalID.Trim();
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                        MapReaderToPerson(reader, person);
                }
            }
            return person;
        }
        public static async Task<PersonData> GetPersonInfoByIDAsync(int id)
        {
            PersonData person = new PersonData();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, 
                                                         People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, People.CountryID,
                                                         People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName
                                                         FROM Countries INNER JOIN People ON Countries.ID = People.CountryID
                                                         WHERE People.ID = @PersonID", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = id;
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                        if (await reader.ReadAsync().ConfigureAwait(false))
                        MapReaderToPerson(reader, person);
                }
            }
            return person;
        }
        public static async Task<PersonData> GetPersonByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return new PersonData();
            PersonData person = new PersonData();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, 
                                                         People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, People.CountryID,
                                                         People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName
                                                         FROM Countries INNER JOIN People ON Countries.ID = People.CountryID
                                                         WHERE Email = @Email", connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email.Trim();
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                        MapReaderToPerson(reader, person);
                }
            }
            return person;
        }
        public static async Task<PersonData> GetPersonByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return new PersonData();
            PersonData person = new PersonData();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, 
                                                         People.ThirdName, People.LastName, People.Gender, People.DateOfBirth, People.CountryID,
                                                         People.Address, People.Phone, People.Email, People.ImagePath, Countries.CountryName
                                                         FROM Countries INNER JOIN People ON Countries.ID = People.CountryID
                                                         WHERE Phone = @Phone", connection))
            {
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone.Trim();
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                        MapReaderToPerson(reader, person);
                }
            }
            return person;
        }
        public static async Task<int> AddNewPersonAsync(PersonData person)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"INSERT INTO People (NationalID,FirstName,SecondName,ThirdName,LastName,Gender,
                                                         DateOfBirth,CountryID,Address,Phone,Email,ImagePath)
                                                         VALUES (@NationalID,@FirstName,@SecondName,@ThirdName,@LastName,@Gender,
                                                         @DateOfBirth,@CountryID,@Address,@Phone,@Email,@ImagePath);
                                                         SELECT CAST(SCOPE_IDENTITY() AS INT);", connection))
            {
                command.Parameters.Add("@NationalID", SqlDbType.NVarChar, 20).Value = person.NationalID;
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = person.FirstName;
                command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 50).Value = person.SecondName;
                command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 50).Value = person.ThirdName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = person.LastName;
                command.Parameters.Add("@Gender", SqlDbType.TinyInt).Value = (byte)person.Gender;
                command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = person.DateOfBirth;
                command.Parameters.Add("@CountryID", SqlDbType.Int).Value = person.CountryID;
                command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = person.Address;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = person.Email;
                command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 255).Value =
                    string.IsNullOrWhiteSpace(person.ImagePath) ? (object)DBNull.Value : person.ImagePath;

                await connection.OpenAsync().ConfigureAwait(false);
                object result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        public static async Task<bool> UpdatePersonAsync(PersonData person)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"UPDATE People SET NationalID=@NationalID,FirstName=@FirstName,SecondName=@SecondName,
                                                         ThirdName=@ThirdName,LastName=@LastName,Gender=@Gender,DateOfBirth=@DateOfBirth,CountryID=@CountryID,
                                                         Address=@Address,Phone=@Phone,Email=@Email,ImagePath=@ImagePath WHERE ID=@PersonID", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = person.PersonID;
                command.Parameters.Add("@NationalID", SqlDbType.NVarChar, 20).Value = person.NationalID;
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = person.FirstName;
                command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 50).Value = person.SecondName;
                command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 50).Value = person.ThirdName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = person.LastName;
                command.Parameters.Add("@Gender", SqlDbType.TinyInt).Value = (byte)person.Gender;
                command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = person.DateOfBirth;
                command.Parameters.Add("@CountryID", SqlDbType.Int).Value = person.CountryID;
                command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = person.Address;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = person.Email;
                command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 255).Value =
                    string.IsNullOrWhiteSpace(person.ImagePath) ? (object)DBNull.Value : person.ImagePath;

                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
            }
        }
        public static async Task<bool> DeletePersonAsync(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("DELETE FROM People WHERE ID = @PersonID", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                await connection.OpenAsync().ConfigureAwait(false);
                try
                {
                    return await command.ExecuteNonQueryAsync().ConfigureAwait(false) > 0;
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    return false;
                }
            }
        }
        public static async Task<bool> IsPersonExistAsync(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM People WHERE ID=@PersonID", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
        public static async Task<bool> IsEmailExistAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM People WHERE Email=@Email", connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email.Trim();
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
        public static async Task<bool> IsNationalIDExistAsync(string NationalID)
        {
            if (string.IsNullOrWhiteSpace(NationalID)) return false;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 1 FROM People WHERE NationalID=@NationalID", connection))
            {
                command.Parameters.Add("@NationalID", SqlDbType.NVarChar, 20).Value = NationalID.Trim();
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
        public static async Task<bool> IsPhoneExistAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT 1 FROM People WHERE Phone=@Phone", connection))
            {
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone.Trim();
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
        public static async Task<List<PersonData>> GetAllPersonsAsync()
        {
            List<PersonData> persons = new List<PersonData>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, People.ThirdName, People.LastName,
                                                         People.Gender, People.DateOfBirth, People.CountryID, People.Address, People.Phone, People.Email, People.ImagePath, 
                                                         Countries.CountryName FROM Countries INNER JOIN People ON Countries.ID = People.CountryID", connection))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        PersonData person = new PersonData();
                        MapReaderToPerson(reader, person);
                        persons.Add(person);
                    }
                }
            }
            return persons;
        }   

        public static async Task<List<PersonData>> SearchPersonsAsync(string value)
        {
            List<PersonData> persons = new List<PersonData>();
            if (string.IsNullOrWhiteSpace(value))
                return persons;

            bool isId = int.TryParse(value.Trim(), out int personId);
            string query = @"SELECT People.ID, People.NationalID, People.FirstName, People.SecondName, People.ThirdName, People.LastName,
                             People.Gender, People.DateOfBirth, People.CountryID, People.Address, People.Phone, People.Email, People.ImagePath,
                             Countries.CountryName
                             FROM People
                             LEFT JOIN Countries ON Countries.ID = People.CountryID
                             WHERE (@PersonID IS NOT NULL AND People.ID = @PersonID)
                                OR People.NationalID LIKE @Value
                                OR People.Phone LIKE @Value
                                OR People.Email LIKE @Value
                                OR (People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName) LIKE @Value
                             ORDER BY People.ID DESC";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = isId ? (object)personId : DBNull.Value;
                command.Parameters.Add("@Value", SqlDbType.NVarChar, 150).Value = "%" + value.Trim() + "%";
                await connection.OpenAsync().ConfigureAwait(false);
                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        PersonData person = new PersonData();
                        MapReaderToPerson(reader, person);
                        persons.Add(person);
                    }
                }
            }

            return persons;
        }

        public static async Task<bool> IsPersonUsedAsync(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(@"IF EXISTS (SELECT 1 FROM Drivers WHERE PersonID = @PersonID)
                                                         OR EXISTS (SELECT 1 FROM Users WHERE PersonID = @PersonID)
                                                         OR EXISTS (SELECT 1 FROM Applications WHERE ApplicantPersonID = @PersonID)
                                                         OR EXISTS (SELECT 1 FROM Violations WHERE PersonID = @PersonID) SELECT 1", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                await connection.OpenAsync().ConfigureAwait(false);
                return await command.ExecuteScalarAsync().ConfigureAwait(false) != null;
            }
        }
    }
}
