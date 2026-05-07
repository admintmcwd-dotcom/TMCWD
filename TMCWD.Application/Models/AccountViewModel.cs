using TMCWD.CustomerSupport;
using TMCWD.Model.CustomerSupport;
using TMCWD.Model.Administrator;

namespace TMCWD.Application.Models
{
    public class AccountViewModel
    {

        private readonly int _customerId;
        private Customer? _customer;
        private List<Account> _accountList;

        public AccountViewModel()
        {
            this._customerId = 0;
            this._customer = new Customer();
            this._accountList = new List<Account>();
        }

        public AccountViewModel(int customerId)
        {
            _customerId = customerId;
            _customer = new Customer();
            _accountList = new List<Account>();
        }

        public User? CurrentUser { get; set; }

        public Customer? Customer
        {
            get
            {
                return this._customer;
            }
        }

        public int CustomerId
        {
            get
            {
                return _customerId;
            }
        }

        public Account AddEditAccount { get; set; } = new Account();

        public List<Account> PagedAccountList { 
            get 
            {
                return _accountList;
            } 
        }

        public void GetCustomerDetails()
        {
            CustomerTransaction trans = new();
            this._customer = trans.GetById(_customerId);
        }

        public void GetAccounts()
        {
            AccountTransaction trans = new();
            this._accountList = trans.GetByCustomerId(this._customerId);
        }

    }
}
