using DVLD.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;

namespace DVLD.BusinessLayer
{
    public sealed class PersonService
    {
        public enum enMode : byte { AddNew = 0, Update = 1 }
        public int PersonID { get; private set; }
        public string NationalID { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string SecondName { get; set; } = "";
        public string ThirdName { get; set; } = "";
        public string LastName { get; set; } = "";
        public GenderType Gender { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public int CountryID { get; set; } = -1;
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public enMode Mode { get; private set; }
        public PersonService()
        {
            PersonID = -1;
            Mode = enMode.AddNew;
        }
        public enum GenderType : byte
        {
            Male = 0,
            Female = 1
        }
        private PersonService(PersonData data)
        {
            PersonID = data.PersonID;
            FirstName = data.FirstName ?? string.Empty;
            SecondName = data.SecondName ?? string.Empty;
            ThirdName = data.ThirdName ?? string.Empty;
            LastName = data.LastName ?? string.Empty;
            Email = data.Email ?? string.Empty;
            NationalID = data.NationalID ?? string.Empty;
            Phone = data.Phone ?? string.Empty;
            Address = data.Address ?? string.Empty;
            ImagePath = data.ImagePath ?? string.Empty;
            Gender = (GenderType)data.Gender;
            DateOfBirth = data.DateOfBirth;
            CountryID = data.CountryID;
            Mode = enMode.Update;
        }
        private PersonData MapToDataObj() => new PersonData
        {
            PersonID = PersonID,
            FirstName = FirstName?.Trim(),
            SecondName = SecondName?.Trim(),
            ThirdName = ThirdName?.Trim(),
            LastName = LastName?.Trim(),
            NationalID = NationalID?.Trim(),
            Email = Email?.Trim(),
            Phone = Phone?.Trim(),
            Address = Address?.Trim(),
            DateOfBirth = DateOfBirth,
            CountryID = CountryID,
            Gender = (byte)Gender,
            ImagePath = ImagePath?.Trim()
        };
        public static async Task<List<PersonListDto>> GetPersonsForList()
        {
            List<PersonData> datalist = await PersonsDataAccess.GetAllPersonsAsync();
            return datalist.Select(person => new PersonListDto
            {
                PersonID = person.PersonID,
                NationalNO = person.NationalID,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName,
                LastName = person.LastName,
                Gender = ((PersonService.GenderType)person.Gender).ToString(),
                DateOfBirth = person.DateOfBirth,
                CountryName = person.CountryName,
                Phone = person.Phone,
                Email = person.Email
            }).ToList();

        }

        public static async Task<List<PersonListDto>> SearchPersonsAsync(string value)
        {
            List<PersonData> dataList = await PersonsDataAccess.SearchPersonsAsync(value);
            return dataList.Select(person => new PersonListDto
            {
                PersonID = person.PersonID,
                NationalNO = person.NationalID,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName,
                LastName = person.LastName,
                Gender = ((GenderType)person.Gender).ToString(),
                DateOfBirth = person.DateOfBirth,
                CountryName = person.CountryName,
                Phone = person.Phone,
                Email = person.Email
            }).ToList();
        }

        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return false;
            if (string.IsNullOrWhiteSpace(LastName)) return false;
            if (string.IsNullOrWhiteSpace(NationalID)) return false;
            if (string.IsNullOrWhiteSpace(Email)) return false;
            if (string.IsNullOrWhiteSpace(Phone)) return false;
            if (CountryID <= 0) return false;
            if (DateOfBirth > DateTime.Today.AddYears(-18)) return false;
            return true;
        }

        public async Task<Result<bool>> ValidatePersonAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return Result<bool>.Fail("First name is required.");
            if (string.IsNullOrWhiteSpace(LastName))
                return Result<bool>.Fail("Last name is required.");
            if (string.IsNullOrWhiteSpace(NationalID))
                return Result<bool>.Fail("National ID is required.");
            if (string.IsNullOrWhiteSpace(Email))
                return Result<bool>.Fail("Email is required.");
            if (string.IsNullOrWhiteSpace(Phone))
                return Result<bool>.Fail("Phone is required.");
            if (CountryID <= 0)
                return Result<bool>.Fail("Valid country is required.");
            if (DateOfBirth > DateTime.Today.AddYears(-18))
                return Result<bool>.Fail("Person must be at least 18 years old.");

            PersonService existingByNationalId = await FindByNationalIDAsync(NationalID);
            if (existingByNationalId != null && existingByNationalId.PersonID != PersonID)
                return Result<bool>.Fail("National ID already exists.");

            PersonService existingByEmail = await FindByEmailAsync(Email);
            if (existingByEmail != null && existingByEmail.PersonID != PersonID)
                return Result<bool>.Fail("Email already exists.");

            PersonService existingByPhone = await FindByPhoneAsync(Phone);
            if (existingByPhone != null && existingByPhone.PersonID != PersonID)
                return Result<bool>.Fail("Phone already exists.");

            return Result<bool>.Ok(true);
        }
        public async Task<Result<bool>> SaveAsync()
        {
            return Mode == enMode.AddNew ? await AddNewPerson() : await UpdatePerson();
        }
        private async Task<Result<bool>> AddNewPerson()
        {
            Result<bool> validation = await ValidatePersonAsync();
            if (!validation.Success) return validation;
            int newId = await PersonsDataAccess.AddNewPersonAsync(MapToDataObj());
            if (newId == -1) return Result<bool>.Fail("Failed to create person in database.");
            PersonID = newId;
            Mode = enMode.Update;
            return Result<bool>.Ok(true, "Person created successfully.");
        }
        private async Task<Result<bool>> UpdatePerson()
        {
            Result<bool> validation = await ValidatePersonAsync();
            if (!validation.Success) return validation;
            bool updated = await PersonsDataAccess.UpdatePersonAsync(MapToDataObj());
            if (!updated) return Result<bool>.Fail("Failed to update person in database.");
            return Result<bool>.Ok(true, "Person updated successfully.");
        }
        public static async Task<PersonService> FindByEmailAsync(string email)
        {
            if (email != null)
            {
                PersonData data = await PersonsDataAccess.GetPersonByEmailAsync(email);
                return data.IsFound ? new PersonService(data) : null;
            }
            return null;
        }
        public static async Task<PersonService> FindByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            PersonData data =
                await PersonsDataAccess.GetPersonByPhoneAsync(phone);
            return data.IsFound ? new PersonService(data) : null;
        }
        public static async Task<PersonService> FindByNationalIDAsync(string nationalID)
        {
            if (string.IsNullOrWhiteSpace(nationalID))
                return null;

            PersonData data = await PersonsDataAccess.GetPersonByNationalIDAsync(nationalID);
            return data.IsFound ? new PersonService(data) : null;
        }


        public static async Task<PersonService> FindByIDAsync(int PersonID)
        {
            if (PersonID <= 0) return null;
            PersonData data = await PersonsDataAccess.GetPersonInfoByIDAsync(PersonID);
            return data.IsFound ? new PersonService(data) : null;
        }
        public static async Task<Result<bool>> DeletePerson(int PersonID)
        {
            Result<bool> canDelete = await CanDeletePersonAsync(PersonID);
            if (!canDelete.Success) return canDelete;
            bool deleted = await PersonsDataAccess.DeletePersonAsync(PersonID);
            if (!deleted) return Result<bool>.Fail("Failed to delete person from database.");
            return Result<bool>.Ok(true, "Person deleted successfully.");
        }

        public static async Task<Result<bool>> CanDeletePersonAsync(int personID)
        {
            if (personID <= 0) return Result<bool>.Fail("Invalid person ID.");
            if (!await PersonsDataAccess.IsPersonExistAsync(personID))
                return Result<bool>.Fail("Person does not exist.");
            if (await PersonsDataAccess.IsPersonUsedAsync(personID))
                return Result<bool>.Fail("Cannot delete person because they are linked to other records (driver, user, application, or violation).");
            return Result<bool>.Ok(true, "Person can be deleted.");
        }
        public static Task<bool> ExistsNationalIDAsync(string nationalID)
        {
            return PersonsDataAccess.IsNationalIDExistAsync(nationalID);
        }
        public static Task<bool> ExistsByIdAsync(int personID) =>
            personID > 0 ? PersonsDataAccess.IsPersonExistAsync(personID) : Task.FromResult(false);
        public static Task<bool> ExistsByPhoneAsync(string phone) =>
            string.IsNullOrWhiteSpace(phone) ? Task.FromResult(false) : PersonsDataAccess.IsPhoneExistAsync(phone);
        public static Task<bool> ExistsByEmailAsync(string email) => //expression-bodied member
            string.IsNullOrWhiteSpace(email) ? Task.FromResult(false) : PersonsDataAccess.IsEmailExistAsync(email);
        public static async Task<List<PersonService>> GetAllPersons()
        {
            List<PersonData> dataList = await PersonsDataAccess.GetAllPersonsAsync();
            List<PersonService> persons = new List<PersonService>();
            foreach (var data in dataList)
            {
                persons.Add(new PersonService(data));
            }
            return persons;
        }
    }
}
