using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TMCWD.Application.Models;
using TMCWD.Model.Administrator;
using TMCWD.Administration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

namespace TMCWD.Application.Controllers
{
    public class AdminController : Controller
    {

        private readonly HttpClient _client;
        
        public AdminController(IHttpClientFactory factory) 
        {
            _client = factory.CreateClient("TmcWdApi");
        }

        public async Task<IActionResult> Index(string searchString = "")
        {
            User? currentUser = new();
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");

            UserTransaction userTransact = new(_client);

            currentUser = userTransact.ConvertJsonStringToUser(jsonCurrentUser);

            var userList = await userTransact.SearchUser(searchString);
            if (userList == null) userList = await userTransact.GetUsers();

            AdminViewModel model = new()
            {
                CurrentUser = currentUser,
                PagedUserList = userList ?? new List<User>(),
                SearchString = searchString
            };

            ViewBag.Role = currentUser?.Role ?? 0;
            return View(model);
        }

        public IActionResult Users()
        {
            return View();
        }

        public async Task<IActionResult> AddEditUser(int currentUserId, int editUserId = 0)
        {

            User? editUser = new();
            User currentUser = new();
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            UserTransaction userTrans = new(_client);

            if (!string.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = userTrans.ConvertJsonStringToUser(jsonCurrentUser);
            }

            if (editUserId > 0)
            {
                
                editUser = await userTrans.Get(editUserId);
            }

            AdminViewModel model = new()
            {
                CurrentUser = currentUser,
                AddEditUser = editUser,
                ConfirmPassword = editUser?.Id > 0 ? editUser.Password : string.Empty,
                Password = editUser?.Id > 0 ? editUser.Password : string.Empty
            };


            model.Roles = userTrans.GetRoles();

            ViewBag.Role = currentUser.Role;
            return View(model);
        }

        [HttpPost()]
        public async Task<IActionResult> SaveUser(AdminViewModel model)
        {
            UserTransaction userTrans = new(_client);
            if(model.CurrentUser == null || model.CurrentUser.Id <= 0)
            {
                var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
                model.CurrentUser = userTrans.ConvertJsonStringToUser(jsonCurrentUser);
            }

            if (model.AddEditUser == null)
                return View("AddEditUser");

            
            await userTrans.SaveUpdate(model.CurrentUser.Id, model.AddEditUser);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> DeactivateUser(int userId)
        {
            UserTransaction userTrans = new(_client);

            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = userTrans.ConvertJsonStringToUser(jsonCurrentUser);

            var user = await userTrans.Get(userId);
            user?.IsActive = false;
            await userTrans.SaveUpdate(currentUser.Id, user);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> InspectionTypes()
        {
            InspectionTypeViewModel model = new();
            UserTransaction userTrans = new(_client);
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();
            currentUser = userTrans.ConvertJsonStringToUser(jsonCurrentUser);

            InspectionTypeTransaction inspectionTypeTrans = new(_client);
            var inspectionTypes = await inspectionTypeTrans.GetTypes();
            if (inspectionTypes != null && inspectionTypes.Any()) model.InspectionTypes = inspectionTypes;

            return View(model);
        }

        public async Task<IActionResult> AddEditInspectionType(int inspectionTypeId = 0)
        {
            InspectionTypeViewModel model = new();
            UserTransaction userTrans = new(_client);

            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");

            model.CurrentUser = userTrans.ConvertJsonStringToUser(jsonCurrentUser);

            model.AddEditInspectionType = new();
            if (inspectionTypeId > 0)
            {
                InspectionTypeTransaction inspectionTypeTrans = new(_client);
                model.AddEditInspectionType = await inspectionTypeTrans.Get(inspectionTypeId);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUpdateInspectionType(InspectionTypeViewModel model)
        {
            InspectionTypeTransaction inspTrans = new(_client);
            UserTransaction userTrans = new(_client);
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();
            if (!string.IsNullOrEmpty(jsonCurrentUser.Trim())){
                currentUser = userTrans.ConvertJsonStringToUser(jsonCurrentUser);
            }
            if(model.AddEditInspectionType.Id <= 0)
            {
                model.AddEditInspectionType.CreatedBy = currentUser.Id;
                model.AddEditInspectionType.DateCreated = DateTime.Now;
            }
            else
            {
                model.AddEditInspectionType.DateUpdated = DateTime.Now;
                model.AddEditInspectionType.UpdatedBy = currentUser.Id;
            }
            await inspTrans.SaveUpdate(currentUser.Id, model.AddEditInspectionType);
            return RedirectToAction("InspectionTypes", "Admin");
        }

        public async Task<IActionResult> DeactivateInspectionType(int inspectionTypeId, int currentUserId)
        {
            InspectionTypeTransaction inspTrans = new(_client);
            InspectionType inspType = new();
            inspType = await inspTrans.Get(inspectionTypeId);
            if (inspType != null)
            {
                inspType.IsActive = false;
                inspType.UpdatedBy = currentUserId;
                inspType.DateUpdated = DateTime.Now;
                await inspTrans.SaveUpdate(currentUserId, inspType);
            }
            return RedirectToAction("InspectionTypes", "Admin");
        }

    }
}
