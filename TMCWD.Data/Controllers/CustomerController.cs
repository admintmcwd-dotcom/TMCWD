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
        public bool SaveUpdate(Customer customer)
        {
            bool isSuccess = false;
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
                    dbContext.SaveChanges();
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                throw;
            }
            return isSuccess;
        }

        [HttpGet("GetById")]
        public Customer GetById(int id)
        {
            Customer customer = new();
            try
            {
                using (var dbcContext = new UserDbContext())
                {
                    var customerEnt = dbcContext.Customers.Where(x => x.Id == id).SingleOrDefault();
                    if(customer == null) throw new Exception($"Customer with id {id} cannot be found.");
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
                throw;
            }

            return customer;
        }

        [HttpGet("GetByName")]
        public Customer GetByName(string firstname, string lastname)
        {
            Customer customer = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var customerEnt = dbContext.Customers.Where(x => x.Firstname.ToLower() == firstname.ToLower() && x.Lastname.ToLower() == lastname.ToLower()).SingleOrDefault();
                    if (customerEnt == null) throw new Exception($"Customer with name {firstname} {lastname} cannot be found.");
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
                throw;
            }
            return customer;

        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new();

            try
            {
                using(var dbcContext = new UserDbContext())
                {
                    customers = dbcContext.Customers.ToList();
                    if(customers.Count == 0) throw new Exception("No customers found.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                throw;
            }

            return customers;
        }
    }
}
