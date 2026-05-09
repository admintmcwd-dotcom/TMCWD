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

        public IActionResult Index(string searchString = "")
        {
            User currentUser = new();
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                 currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
            }

            UserTransaction userTransact = new();
            var userList = userTransact.SearchUser(searchString) ?? userTransact.GetUsers();

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

        public IActionResult AddEditUser(int currentUserId, int editUserId = 0)
        {

            User? editUser = new();
            User currentUser = new();
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");

            if (!string.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
            }

            if (editUserId > 0)
            {
                UserTransaction userTransact = new();
                editUser = userTransact.GetUserById(editUserId);
            }

            AdminViewModel model = new()
            {
                CurrentUser = currentUser,
                AddEditUser = editUser,
                ConfirmPassword = editUser?.Id > 0 ? editUser.Password : string.Empty,
                Password = editUser?.Id > 0 ? editUser.Password : string.Empty
            };

            UserTransaction userTrans = new();

            model.Roles = userTrans.GetRoles();

            ViewBag.Role = currentUser.Role;
            return View(model);
        }

        [HttpPost()]
        public IActionResult SaveUser(AdminViewModel model)
        {
            UserTransaction userTrans = new();

            if (model?.AddEditUser?.Id > 0)
            {
                userTrans.UpdateUser(model.AddEditUser);
            }
            else
            {
                if (!string.IsNullOrEmpty(model.Password.Trim())) model.AddEditUser.Password = model.Password;
                userTrans.SaveUser(model.AddEditUser, model.ConfirmPassword);
            }

            //return View("AddEditUser", model);
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult DeactivateUser(int userId)
        {
            UserTransaction userTrans = new();
            var user = userTrans.GetUserById(userId);
            user?.IsActive = false;
            userTrans.UpdateUser(user);
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult InspectionTypes()
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            InspectionTypeViewModel model = new();
            User? currentUser = new();
            if (!String.IsNullOrEmpty(jsonCurrentUser?.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                if (currentUser != null)
                {
                    model.CurrentUser = currentUser;
                    ViewBag.Role = currentUser.Role;
                }
            }

            InspectionTypeTransaction inspectionTypeTrans = new();
            var inspectionTypes = inspectionTypeTrans.GetIncidentTypes();
            if (inspectionTypes != null && inspectionTypes.Any()) model.InspectionTypes = inspectionTypes;

            return View(model);
        }

        public IActionResult AddEditInspectionType(int inspectionTypeId = 0)
        {
            InspectionTypeViewModel model = new();

            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");

            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                model.CurrentUser = new();
                model.CurrentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                ViewBag.Role = model.CurrentUser.Role;
            }

            model.AddEditInspectionType = new();
            if (inspectionTypeId > 0)
            {
                InspectionTypeTransaction inspectionTypeTrans = new();
                model.AddEditInspectionType = inspectionTypeTrans.GetInspectionTypeById(inspectionTypeId);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveUpdateInspectionType(InspectionTypeViewModel model)
        {
            InspectionTypeTransaction inspTrans = new();
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();
            if (!string.IsNullOrEmpty(jsonCurrentUser.Trim())){
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
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
            inspTrans.SaveUpdateInspectionType(model.AddEditInspectionType);
            return RedirectToAction("InspectionTypes", "Admin");
        }

        public IActionResult DeactivateInspectionType(int inspectionTypeId, int currentUserId)
        {
            InspectionTypeTransaction inspTrans = new();
            InspectionType inspType = new();
            inspType = inspTrans.GetInspectionTypeById(inspectionTypeId);
            if (inspType != null)
            {
                inspType.IsActive = false;
                inspType.UpdatedBy = currentUserId;
                inspType.DateUpdated = DateTime.Now;
                inspTrans.SaveUpdateInspectionType(inspType);
            }
            return RedirectToAction("InspectionTypes", "Admin");
        }

    }
}
