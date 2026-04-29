using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [HttpPost("SaveUser")]
        public ActionResult<bool> Save([FromBody] User user)
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {

                    var res = dbContext.Users.Where(x => x.Email.ToLower() == user.Email.ToLower()).FirstOrDefault();

                    if (res != null) return BadRequest($"User with email id {user.Email} already exists");

                    dbContext.Users.Add(user);
                    int resp = dbContext.SaveChanges();
                    if (resp > 0) return Ok(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(false);
        }

        [HttpGet("GetById")]
        public ActionResult<User> Get(int id)
        {
            User user = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Id == id).SingleOrDefault();
                    if (userEnt == null) return NotFound($"User with id {id.ToString()} is not found");
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
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }
            return Ok(user);
        }

        [HttpGet("GetByName")]
        public ActionResult<User> GetByName(string name)
        {
            User user = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Name.ToLower() == name.ToLower()).SingleOrDefault();
                    if (userEnt == null) return NotFound($"User with name {name} is not found");
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
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }
            return Ok(user);
        }

        [HttpGet("GetByEmail")]
        public ActionResult<User> GetByEmail(string email)
        {
            User user = new();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Email.ToLower() == email.ToLower()).SingleOrDefault();
                    if (userEnt == null) return NotFound($"User with email {email} is not found.");
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
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(user);
        }

        [HttpGet("GetUsers")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> users = new List<User>();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    users = dbContext.Users;
                    if (!users.Any()) return NotFound("No user records found");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }
            return Ok(users);
        }

        [HttpPut("UpdateUser")]
        public ActionResult<bool> Update([FromBody] User user)
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var usersEnt = dbContext.Users.Where(x => x.Id.Equals(user.Id)).SingleOrDefault();
                    if (usersEnt == null) BadRequest($"User with id {user.Id} cannot be found.");
                    usersEnt.DateUpdated = DateTime.Now;
                    usersEnt.IsActive = user.IsActive;
                    usersEnt.DateVerified = user.DateVerified;
                    usersEnt.IsVerified = user.IsVerified;
                    usersEnt.RememberToken = user.RememberToken;
                    usersEnt.Name = user.Name;
                    usersEnt.Email = user.Email;
                    int res = dbContext.SaveChanges();
                    if (res > 0) return Ok(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(false);
        }

        [HttpPut("ChangePassword")]
        public ActionResult<bool> ChangePassword(int id, string newPassword)
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var userEnt = dbContext.Users.Where(x => x.Id.Equals(id)).SingleOrDefault();
                    if (userEnt == null) BadRequest($"User with id {id} cannot be found.");
                    userEnt.Password = newPassword;
                    int res = dbContext.SaveChanges();
                    if (res > 0) return Ok(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }
            return Ok(false);
        }

    }
}