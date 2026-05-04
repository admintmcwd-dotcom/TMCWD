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
        public string AccountNumber { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsCurrentAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
