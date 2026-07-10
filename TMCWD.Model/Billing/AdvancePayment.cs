using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.Billing.Interfaces;

namespace TMCWD.Model.Billing
{
    public class AdvancePayment : IAdvancePayment
    {

        #region constructors

        public AdvancePayment() { }

        #endregion

        #region properties

        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }

        #endregion

    }
}
