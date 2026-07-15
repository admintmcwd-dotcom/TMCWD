using Microsoft.AspNetCore.Mvc;
using TMCWD.Administration;
using TMCWD.Billing;
using TMCWD.Model.Administrator;
using TMCWD.Model.Billing;
using TMCWD.Services;

namespace TMCWD.Application.Controllers
{
    public class ReadingSheetController : Controller
    {
        #region fields

        private readonly AuthenticatedUserService _user;
        private readonly ReadingSheetTransaction _readingSheetTrans;
        private readonly UserTransaction _userTrans;

        #endregion

        #region constructors

        public ReadingSheetController(AuthenticatedUserService user, ReadingSheetTransaction readingSheetTrans, UserTransaction userTrans)
        {
            _user = user;
            _readingSheetTrans = readingSheetTrans;
            _userTrans = userTrans;
        }

        #endregion

        #region methods

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateReadingSheet(int zone, int book, int assignedTo, DateTime billingPeriod)
        {

            var assignedToUser = await _userTrans.Get(assignedTo);

            string name = $"{DateTime.Now.ToString("MM - dd - yyyy")} - {assignedToUser.Name}";

            ReadingSheet sheet = new ReadingSheet
            {
                AssignedTo = assignedTo,
                BillingDate = billingPeriod,
                Book = book,
                Name = name,
                CreatedBy = _user.User.Id,
                Zone = zone,
                DateCreated = DateTime.Now
            };

            var savedSheet = await _readingSheetTrans.SaveUpdate(_user.User.Id, sheet);

            return Ok(savedSheet);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentByAssignedTo(int assignedTo)
        {
            var sheet = await _readingSheetTrans.GetCurrentByAssignedTo(assignedTo);
            return Ok(sheet);
        }

        [HttpGet]
        public async Task<IActionResult> GetByAssignedTo(int assignedTo)
        {
            var readingSheets = await _readingSheetTrans.GetByAssignedTo(assignedTo);
            return Ok(readingSheets);
        }

        #endregion

    }
}
