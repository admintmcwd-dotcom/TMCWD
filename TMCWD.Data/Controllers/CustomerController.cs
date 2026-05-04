using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Customer customer)
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    if (customer.Id == 0)
                    {
                        dbContext.Customers.Add(customer);
                    }
                    else
                    {
                        dbContext.Customers.Update(customer);
                    }
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
        public ActionResult<Customer> GetById(int id)
        {
            Customer customer = new();
            try
            {
                using (var dbcContext = new UserDbContext())
                {
                    var customerEnt = dbcContext.Customers.Where(x => x.Id == id).SingleOrDefault();
                    if(customer == null) return NotFound($"Customer with id {id} cannot be found.");
                    customer.DateCreated = customerEnt?.DateCreated ?? DateTime.MinValue;
                    customer.DateUpdated = customerEnt?.DateUpdated ?? DateTime.MinValue;
                    customer.Email = customerEnt?.Email ?? string.Empty;
                    customer.Id = customerEnt?.Id ?? 0;
                    customer.IsActive = customerEnt?.IsActive ?? false;
                    customer.Firstname = customerEnt?.Firstname ?? string.Empty;
                    customer.Lastname = customerEnt?.Lastname ?? string.Empty;
                    customer.Middlename = customerEnt?.Middlename ?? string.Empty;
                    customer.PhoneNumber = customerEnt?.PhoneNumber ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(customer);
        }

        [HttpGet("GetByName")]
        public ActionResult<Customer> GetByName(string firstname, string lastname)
        {
            Customer customer = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var customerEnt = dbContext.Customers.Where(x => x.Firstname.ToLower() == firstname.ToLower() && x.Lastname.ToLower() == lastname.ToLower()).SingleOrDefault();
                    if (customerEnt == null) return NotFound($"Customer with name {firstname} {lastname} cannot be found.");
                    customer.DateCreated = customerEnt.DateCreated;
                    customer.DateUpdated = customerEnt.DateUpdated;
                    customer.Email = customerEnt.Email;
                    customer.Id = customerEnt.Id;
                    customer.IsActive = customerEnt.IsActive;
                    customer.Firstname = customerEnt.Firstname;
                    customer.Lastname = customerEnt.Lastname;
                    customer.Middlename = customerEnt.Middlename;
                    customer.PhoneNumber = customerEnt.PhoneNumber;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }
            return Ok(customer);

        }

        [HttpGet("GetCustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            IEnumerable<Customer> customers = new List<Customer>();

            try
            {
                using(var dbcContext = new UserDbContext())
                {
                    customers = dbcContext.Customers;
                    if(!customers.Any()) return NotFound("No customers found.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(customers);
        }
    }
}
