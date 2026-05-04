using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Account account)
        {
            try
            {
                using(var dbContext = new UserDbContext())
                {
                    if (account.Id > 0)
                        dbContext.Accounts.Update(account);
                    else
                        dbContext.Accounts.Add(account);

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

        [HttpGet("GetById")]
        public ActionResult<Account> GetById(int id)
        {
            Account account = new();

            try
            {
                using (UserDbContext dbContext = new UserDbContext())
                {
                    var data = dbContext.Accounts.Where(x => x.Id == id).FirstOrDefault();
                    if (data == null) return NotFound($"Account with id {id} not found");
                    account = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(account);
        }

        [HttpGet("GetByAccountNumber")]
        public ActionResult<Account> GetByAccountNumber(string accountNumber)
        {
            Account account = new();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Accounts.Where(x => x.AccountNumber.ToLower() == accountNumber.ToLower()).FirstOrDefault();
                    if (data == null) return NotFound($"Account with account number {accountNumber} not found");
                    account = data;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(account);
        }

        [HttpGet("GetByCustomerId")]
        public ActionResult<IEnumerable<Account>> GetByCustomerId(int customerId)
        {
            IEnumerable<Account> accounts = new List<Account>();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Accounts.Where(x => x.CustomerId == customerId);
                    if (!data.Any()) return NotFound($"Account(s) with customer id {customerId} not found");
                    accounts = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(accounts);
        }

        [HttpGet("GetByMeterNumber")]
        public ActionResult<Account> GetByMeterNumber(string meterNumber)
        {
            Account account = new();
            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var data = dbContext.Accounts.Where(x => x.MeterNumber.ToLower().Trim() == meterNumber.ToLower().Trim()).FirstOrDefault();
                    if (account == null) return NotFound($"Account with meter number {meterNumber} is not found.");
                    account = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(account);
        }
    }
}
