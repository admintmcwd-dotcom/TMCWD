using TMCWD.Model.Administrator;
using TMCWD.Model.CustomerSupport;
namespace TMCWD.Application.Models
{
    public class RequestViewModel
    {

        public User CurrentUser { get; set; } = new User();

        public string SearchString { get; set; } = string.Empty;

        public Request AddEditRequest { get; set; } = new Request();

        public List<Request>Requests { get; set; } = new List<Request>();

        public List<InspectionType> InspectionTypes { get; set; } = new List<InspectionType>();

        public Customer CurrentCustomer { get; set; } = new Customer();

        public Account CurrentAccount { get; set; } = new Account();

    }
}
