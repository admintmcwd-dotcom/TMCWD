using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private readonly UserDbContext _dbContext;

        public AccountController(UserDbContext context)
        {
            this._dbContext = context;
        }

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Account account)
        {
            if (account.Id > 0)
                _dbContext.Accounts.Update(account);
            else
                _dbContext.Accounts.Add(account);

            int res = _dbContext.SaveChanges();
            if (res > 0) return Ok(true);
            return Ok(false);
        }

        [HttpGet("GetById")]
        public ActionResult<Account> GetById(int id)
        {
            Account account = new();

            var data = _dbContext.Accounts.Where(x => x.Id == id).FirstOrDefault();
            if (data == null) return NotFound($"Account with id {id} not found");
            account = data;

            return Ok(account);
        }

        [HttpGet("GetByAccountNumber")]
        public ActionResult<Account> GetByAccountNumber(string accountNumber)
        {
            Account account = new();

            var data = _dbContext.Accounts.Where(x => x.AccountNumber.ToLower() == accountNumber.ToLower()).FirstOrDefault();
            if (data == null) return NotFound($"Account with account number {accountNumber} not found");
            account = data;

            return Ok(account);
        }

        [HttpGet("GetByCustomerId")]
        public ActionResult<IEnumerable<Account>> GetByCustomerId(int customerId)
        {
            IEnumerable<Account> accounts = new List<Account>();

            var data = _dbContext.Accounts.Where(x => x.CustomerId == customerId);
            if (!data.Any()) return NotFound($"Account(s) with customer id {customerId} not found");
            accounts = data;

            return Ok(accounts);
        }

        [HttpGet("GetByMeterNumber")]
        public ActionResult<Account> GetByMeterNumber(string meterNumber)
        {
            Account account = new();

            var data = _dbContext.Accounts.Where(x => x.MeterNumber.ToLower().Trim() == meterNumber.ToLower().Trim()).FirstOrDefault();
            if (data == null) return NotFound($"Account with meter number {meterNumber} is not found.");
            account = data;

            return Ok(account);
        }
    }
}
