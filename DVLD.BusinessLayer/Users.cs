using DVLD.DataAccess;
using DVLD.DataAccessLayer;
using DVLD.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public class UserService
    {
        public int ID { get; private set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; private set; }

        private string _PasswordHash;
        public bool IsFound { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;
        internal UserService()
        {
            ID = -1;
            PersonID = -1;
            UserName = string.Empty;
            _PasswordHash = string.Empty;
            IsActive = true;
            _Mode = enMode.AddNew;
        }
        public UserService(UserData data)
        {
            ID = data.ID;
            PersonID = data.PersonID;
            UserName = data.UserName;
            _PasswordHash = data.PasswordHash;
            IsActive = data.IsActive;
            LastLogin = data.LastLogin;
            CreatedAt = data.CreatedAt;
            _Mode = enMode.Update;
        }
        internal UserData ToData()
        {
            return new UserData
            {
                ID = this.ID,
                PersonID = this.PersonID,
                UserName = this.UserName,
                PasswordHash = this._PasswordHash,
                IsActive = this.IsActive,
                LastLogin = this.LastLogin
            };
        }
        public void SetPassword(string password)
        {
            _PasswordHash = PasswordHasher.HashPassword(password);
        }
        internal string GetPasswordHash()
        {
            return _PasswordHash;
        }
        public static async Task<Result<int>> RegisterUserAsync(string userName, string password, int personID)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return Result<int>.Fail("Username and password are required");
            if (!await PersonsDataAccess.IsPersonExistAsync(personID))
                return Result<int>.Fail("Person does not exist");
            if (await UsersDataAccess.UserNameExistsAsync(userName))
                return Result<int>.Fail("Username already exists");
            UserService user = new UserService();
            user.UserName = userName;
            user.PersonID = personID;
            user.SetPassword(password);
            int id = await UsersDataAccess.AddAsync(user.ToData());
            if (id == -1)
                return Result<int>.Fail("Failed to create user");
            return Result<int>.Ok(id, "User created successfully");
        }
        public static async Task<UserService> FindByIDAsync(int id)
        {
            UserData data = await UsersDataAccess.GetByIDAsync(id);
            return data != null ? new UserService(data) : null;
        }
        public static async Task<UserService> FindByUserNameAsync(string userName)
        {
            UserData data = await UsersDataAccess.GetByUserNameAsync(userName);
            return data != null ? new UserService(data) : null;
        }
        public static async Task<bool> IsUserNameExistAsync(string userName)
        {
            return await UsersDataAccess.UserNameExistsAsync(userName);
        }
        public static async Task<bool> ExistsAsync(int id)
        {
            return await UsersDataAccess.ExistsAsync(id);
        }
        public static async Task<List<UserListDto>> GetUsersForListAsync()
        {
            List<UserListData> data = await UsersDataAccess.GetUsersForListAsync();

            return data.Select(x => new UserListDto
            {
                UserID = x.UserID,
                PersonID = x.PersonID,
                FullName = x.FullName,
                UserName = x.UserName,
                IsActive = x.IsActive
            }).ToList();
        }
        public static async Task<List<UserListDto>> SearchUsersAsync(string value)
        {
            List<UserListData> data = await UsersDataAccess.SearchUsersAsync(value);
            return data.Select(x => new UserListDto
            {
                UserID = x.UserID,
                PersonID = x.PersonID,
                FullName = x.FullName,
                UserName = x.UserName,
                IsActive = x.IsActive
            }).ToList();
        }
        public static async Task<Result<bool>> ValidateUserAsync(string userName, int personId, int currentUserId = -1)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return Result<bool>.Fail("Username is required.");
            if (personId <= 0)
                return Result<bool>.Fail("Valid person ID is required.");
            if (!await PersonsDataAccess.IsPersonExistAsync(personId))
                return Result<bool>.Fail("Person does not exist.");

            UserService existingUser = await FindByUserNameAsync(userName);
            if (existingUser != null && existingUser.ID != currentUserId)
                return Result<bool>.Fail("Username already exists.");

            if (currentUserId <= 0 && await UsersDataAccess.ExistsByPersonIDAsync(personId))
                return Result<bool>.Fail("This person already has a user account.");

            return Result<bool>.Ok(true);
        }
        public static Task<Result<bool>> CanCreateUserAsync(string userName, int personId)
        {
            return ValidateUserAsync(userName, personId);
        }
        public async Task<Result<bool>> SaveAsync()
        {
            Result<bool> validation = await ValidateUserAsync(UserName, PersonID, ID);
            if (!validation.Success) return validation;
            if (_Mode == enMode.AddNew)
            {
                this.ID = await UsersDataAccess.AddAsync(ToData());
                if (this.ID != -1)
                {
                    _Mode = enMode.Update;
                    return Result<bool>.Ok(true, "User saved successfully.");
                }
                return Result<bool>.Fail("Failed to create user in database.");
            }
            else
            {
                bool updated = await UsersDataAccess.UpdateAsync(ToData());
                if (!updated) return Result<bool>.Fail("Failed to update user in database.");
                return Result<bool>.Ok(true, "User updated successfully.");
            }
        }
        public async Task<Result<bool>> DeleteAsync()
        {
            if (this.ID == -1) return Result<bool>.Fail("Invalid user ID.");
            bool deleted = await UsersDataAccess.DeleteAsync(this.ID);
            if (!deleted) return Result<bool>.Fail("Failed to delete user from database.");
            return Result<bool>.Ok(true, "User deleted successfully.");
        }
        public async Task<Result<bool>> ChangePasswordAsync(string password)
        {
            if (this.ID == -1) return Result<bool>.Fail("Invalid user ID.");
            if (string.IsNullOrWhiteSpace(password)) return Result<bool>.Fail("Password cannot be empty.");

            string hashed = PasswordHasher.HashPassword(password);
            bool result = await UsersDataAccess.UpdatePasswordAsync(this.ID, hashed);
            if (!result) return Result<bool>.Fail("Failed to change password.");
            _PasswordHash = hashed;
            return Result<bool>.Ok(true, "Password changed successfully.");
        }
        public Task<Result<bool>> ActivateUserAsync()
        {
            return SetActiveAsync(true);
        }
        public Task<Result<bool>> DeactivateUserAsync()
        {
            return SetActiveAsync(false);
        }
        public async Task<Result<bool>> SetActiveAsync(bool isActive)
        {
            if (this.ID == -1) return Result<bool>.Fail("Invalid user ID.");
            bool result = await UsersDataAccess.SetActiveAsync(this.ID, isActive);
            if (!result) return Result<bool>.Fail("Failed to update user active status.");
            this.IsActive = isActive;
            return Result<bool>.Ok(true, "User active status updated successfully.");
        }
        public static async Task<UserService> LoginAsync(string userName, string password)
        {
            UserData user = await UsersDataAccess.GetByUserNameAsync(userName);
            if (user == null || !user.IsActive)
                return null;
            try
            {
                if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                    return null;
            }
            catch
            {
                return null;
            }
            await UsersDataAccess.UpdateLastLoginAsync(user.ID);
            return new UserService(user);
        }
    }
}
