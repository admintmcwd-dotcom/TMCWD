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

    }
}
