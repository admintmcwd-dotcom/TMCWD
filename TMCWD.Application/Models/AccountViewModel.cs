using TMCWD.CustomerSupport;
using TMCWD.Model.CustomerSupport;
using TMCWD.Model.Administrator;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TMCWD.Application.Models
{
    public class AccountViewModel
    {

        private readonly int _customerId;

        public AccountViewModel()
        {
            this._customerId = 0;
        }

        public AccountViewModel(int customerId)
        {
            _customerId = customerId;
        }

        public User? CurrentUser { get; set; }

        public Customer Customer { get; set; }

        public int CustomerId
        {
            get
            {
                return _customerId;
            }
        }

        public Account AddEditAccount { get; set; } = new Account();

        public List<Account> PagedAccountList { get; set; }

        public List<SelectListItem> AccountStatuses { get; set; } = new List<SelectListItem>() 
        { 
            new SelectListItem { Text = AccountStatus.Pending.ToString(), Value = $"{(int)AccountStatus.Pending}" },
            new SelectListItem { Text = AccountStatus.Active.ToString(), Value = $"{(int)AccountStatus.Active}" },
            new SelectListItem { Text = AccountStatus.Suspended.ToString(), Value = $"{(int)AccountStatus.Suspended}" },
            new SelectListItem { Text = AccountStatus.Closed.ToString(), Value = $"{(int)AccountStatus.Closed}" }
        };
    }
}
