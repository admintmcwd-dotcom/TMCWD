using TMCWD.Data.Context;
using TMCWD.Model.Administrator;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Test
{
    public class TestUser
    {

        public TestUser() { }

        public User GetByEmail(string email)
        {
            User user = new();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Email == email).SingleOrDefault();
                    if (userEnt == null) throw new Exception($"User with email {email} is not found.");
                    user.DateCreated = userEnt.DateCreated;
                    user.DateUpdated = userEnt?.DateUpdated ?? DateTime.MinValue;
                    user.DateVerified = userEnt?.DateVerified ?? DateTime.MinValue;
                    user.Email = userEnt?.Email ?? String.Empty;
                    user.Id = userEnt?.Id ?? 0;
                    user.IsActive = userEnt.IsActive;
                    user.IsVerified = userEnt.IsVerified;
                    user.Name = userEnt.Name;
                    user.Password = userEnt.Password;
                    user.RememberToken = userEnt.RememberToken;
                    user.Role = userEnt.Role;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
            }

            return user;
        }

    }
}
