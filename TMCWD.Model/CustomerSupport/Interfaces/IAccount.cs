using System;
using System.Collections.Generic;
using System.Text;

namespace TMCWD.Model.CustomerSupport.Interfaces
{
    public interface IAccount
    {

        #region properties

        public System.Int64 Id { get; set; }

        public System.Int64 CustomerId { get; set; }

        public string AccountNumber { get; set; }

        public string MeterNumber { get; set; }

        public string Address { get; set; }

        public bool IsCurrentAddress { get; set; }
        
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsActive { get; set; }

        #endregion

    }
}
