using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {

        private readonly UserDbContext _dbContext;

        public CustomerController(UserDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Customer customer)
        {
            if (customer.Id == 0)
            {
                _dbContext.Customers.Add(customer);
            }
            else
            {
                _dbContext.Customers.Update(customer);
            }
            int res = _dbContext.SaveChanges();
            if (res > 0) return Ok(true);

            return Ok(false);
        }

        [HttpGet("GetById")]
        public ActionResult<Customer> GetById(int id)
        {
            var customerEnt = _dbContext.Customers.Where(x => x.Id == id).SingleOrDefault();
            if (customerEnt == null) return NotFound($"Customer with id {id} cannot be found.");
            return Ok(customerEnt);
        }

        [HttpGet("GetByName")]
        public ActionResult<Customer> GetByName(string firstname, string lastname)
        {
            var customerEnt = _dbContext.Customers.Where(x => x.Firstname.ToLower() == firstname.ToLower() && x.Lastname.ToLower() == lastname.ToLower()).SingleOrDefault();
            if (customerEnt == null) return NotFound($"Customer with name {firstname} {lastname} cannot be found.");
            return Ok(customerEnt);

        }

        [HttpGet("GetCustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _dbContext.Customers;
            if (!customers.Any()) return NotFound("No customers found.");

            return Ok(customers);
        }

    }
}
