using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentTypeController : Controller
    {
        [HttpGet("GetTypes")]
        public List<InspectionType> GetTypes()
        {
            List<InspectionType> types = new();

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var res = dbContext.InspectionTypes.Select(x => new InspectionType
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DateCreated = x.DateCreated,
                        DateUpdated = x.DateUpdated
                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return types;
        }

        [HttpGet("GetById")]
        public InspectionType Get(int id)
        {
            InspectionType type = new();

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var res = dbContext.InspectionTypes.Where(x => x.Id == id).FirstOrDefault();
                    if (res == null) throw new Exception($"Incident type id {id} not found");

                    type.Id = res.Id;
                    type.Name = res.Name;
                    type.DateCreated = res.DateCreated;
                    type.DateUpdated = res.DateUpdated;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return type;
        }

        [HttpPost("SaveUpdate")]
        public bool SaveUpdate(InspectionType type)
        {
            bool isSuccess = false;

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    if (type.Id == 0)
                    {
                        dbContext.InspectionTypes.Add(new Entities.InspectionType
                        {
                            Name = type.Name,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now
                        });
                    }
                    else
                    {
                        var res = dbContext.InspectionTypes.Where(x => x.Id == type.Id).FirstOrDefault();
                        if (res == null) throw new Exception($"Incident type id {type.Id} not found");
                        res.Name = type.Name;
                        res.DateUpdated = DateTime.Now;
                    }
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }
    }
}
