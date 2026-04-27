using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Model.Administrator;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [HttpPost("SaveUser")]
        public bool Save(User user)
        {
            bool isSuccess = false;
            try
            {
                using (var dbContext = new UserDbContext())
                {

                    Users userEntity = new()
                    {
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        Email = user.Email,
                        Name = user.Name,
                        Password = user.Password,
                        Role = (int)user.Role,
                        IsVerified = false
                    };
                    dbContext.Users.Add(userEntity);
                    dbContext.SaveChanges();
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        [HttpGet("GetById")]
        public User Get(int id)
        {
            User user = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Id.Equals(id)).SingleOrDefault();
                    if (userEnt == null) throw new Exception($"User with id {id} cannot be found.");
                    user.DateCreated = userEnt.DateCreated;
                    user.DateUpdated = userEnt.DateUpdated;
                    user.DateVerified = userEnt.DateVerified;
                    user.Email = userEnt.Email;
                    user.Id = userEnt.Id;
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

        [HttpGet("GetByName")]
        public User GetByName(string name)
        {
            User user = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (userEnt == null) throw new Exception($"User with name {name} cannot be found.");
                    user.DateCreated = userEnt.DateCreated;
                    user.DateUpdated = userEnt.DateUpdated;
                    user.DateVerified = userEnt.DateVerified;
                    user.Email = userEnt.Email;
                    user.Id = userEnt.Id;
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

        [HttpGet("GetByEmail")]
        public User GetByEmail(string email)
        {
            User user = new();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (userEnt == null) throw new Exception($"User with email {email} is not found.");
                    user.DateCreated = userEnt.DateCreated;
                    user.DateUpdated = userEnt.DateUpdated;
                    user.DateVerified = userEnt.DateVerified;
                    user.Email = userEnt.Email;
                    user.Id = userEnt.Id;
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

        [HttpGet("GetUsers")]
        public List<User> GetUsers()
        {
            List<User> users = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    users = dbContext.Users.Cast<User>().ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
            }
            return users;
        }

        [HttpPut("UpdateUser")]
        public bool Update(User user)
        {
            bool isSuccess = false;

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var usersEnt = dbContext.Users.Where(x => x.Id.Equals(user.Id)).SingleOrDefault();
                    if(usersEnt == null) throw new Exception("User with id {user.Id} cannot be found.");
                    usersEnt.DateUpdated = DateTime.Now;
                    usersEnt.IsActive = user.IsActive;
                    usersEnt.DateVerified = user.DateVerified;
                    usersEnt.IsVerified = user.IsVerified;
                    usersEnt.RememberToken = user.RememberToken;
                    usersEnt.Name = user.Name;
                    usersEnt.Email = user.Email;
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        [HttpPut("ChangePassword")]
        public bool ChangePassword(int id, string newPassword) 
        {
            bool isSuccess = false;
            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Id.Equals(id)).SingleOrDefault();
                    if(userEnt == null) throw new Exception("User with id {id} cannot be found.");
                    userEnt.Password = newPassword;
                    dbContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
            }
            return isSuccess;
        }

    }
}
