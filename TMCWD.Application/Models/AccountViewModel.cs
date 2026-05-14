using TMCWD.CustomerSupport;
using TMCWD.Model.CustomerSupport;
using TMCWD.Model.Administrator;

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

    }
}
