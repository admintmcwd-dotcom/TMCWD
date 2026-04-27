using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.CustomerSupport.Interfaces;

namespace TMCWD.Model.CustomerSupport
{
    public class Account : IAccount
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
