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

        public string DateCreated { get; set; }

        public string DateUpdated { get; set; }

        public bool IsActive { get; set; }

        #endregion

    }
}
