using DVLD.BusinessLayer;
using DVLD.DataAccess;

namespace DVLD.BusinessLayer
{
    public static class UserMapper
    {
        public static UserData ToData(string userName, string passwordHash, int personID)
        {
            return new UserData
            {
                UserName = userName,
                PasswordHash = passwordHash,
                PersonID = personID,
                IsActive = true,
            };
        }
        public static UserService ToBusiness(UserData data)
        {
            return new UserService(data);
        }

        public static UserDto ToDTO(UserService user)
        {
            return new UserDto
            {
                ID = user.ID,
                PersonID = user.PersonID,
                UserName = user.UserName,
                IsActive = user.IsActive,
                LastLogin = user.LastLogin
                
            };
        }
        public static UserDto ToDTO(UserData data)  //performance
        {
            return new UserDto
            {
                ID = data.ID,
                PersonID = data.PersonID,
                UserName = data.UserName,
                IsActive = data.IsActive,
                LastLogin = data.LastLogin
            };
        }
    }
}
