using System;
using System.Text;

namespace TMCWD.Model.Billing.Interfaces
{
    public interface IAdvancePayment
    {

        public int Id { get; set; }

        public int AccountId { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public System.Int64 CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public System.Int64 UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }

    }
}
