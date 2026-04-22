using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Data.Utilities;
using TMCWD.Model.Administrator;

namespace TMCWD.Data.Administrator
{
    public class UserData
    {

        #region constructors
        public UserData() { }
        #endregion

        #region public methods

        public User Get(int id)
        {
            User user = new User();
            try
            {
                using var context = new UserDbContext();

            }
            catch { }
            return user;
        }

        public bool Save(User user)
        {
            bool isSuccess = false;
            try
            {
                using var context = new UserDbContext();

                UserEntity userEnt = new UserEntity
                {
                    Created_At = DateTime.Now,
                    Email = user.Email,
                    Name = user.Name,
                    Password = user.Password,
                    Remember_Token = string.Empty,
                    Role = user.Role,
                    Updated_At = DateTime.Now
                };

                context.UserEntities.Add(userEnt);
                int id = context.SaveChanges();
                if (id > 0) isSuccess = true;
                
            }
            catch { }

            return isSuccess;
        }

        public bool Update(User user)
        {
            bool isSuccess = false;

            try
            {
                UserEntity? userEnt = this.GetById(user.Id);
                if (userEnt == null) throw new Exception("Cannot find user");

                using var context = new UserDbContext();
                userEnt.Updated_At = DateTime.Now;
                userEnt.Email = user.Email;
                userEnt.Name = user.Name;
                userEnt.Password = user.Password;
                userEnt.Remember_Token = user.Token;
                int id = context.SaveChanges();
                if (id > 0) isSuccess = true;
            }
            catch { }

            return isSuccess;
        }

        public bool UpdateToken(int userId, string token)
        {
            bool isSuccess = false;

            try
            {
                var userEnt = this.GetById(userId);
                if (userEnt == null) throw new Exception("User not found");

                using var context = new UserDbContext();

                userEnt.Updated_At = DateTime.Now;
                userEnt.Remember_Token = token;

                context.SaveChanges();
            }
            catch { }

            return isSuccess;
        }

        #endregion

        #region private methods

        private UserEntity? GetById(int id)
        {
            UserEntity? userEnt = new UserEntity();

            try
            {
                using var context = new UserDbContext();
                userEnt = context.UserEntities.Find(id);
            }
            catch { }

            return userEnt;
        }

        #endregion
    }
}
