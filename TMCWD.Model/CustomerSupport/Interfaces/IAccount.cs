using System;
using System.Collections.Generic;
using System.Text;

namespace TMCWD.Model.CustomerSupport.Interfaces
{
    public interface IAccount
    {

        #region properties

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string AccountNumber { get; set; }

        public string MeterNumber { get; set; }

        public string Address { get; set; }

        public bool IsCurrentAddress { get; set; }
        
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public AccountStatus Status { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

    }
}
