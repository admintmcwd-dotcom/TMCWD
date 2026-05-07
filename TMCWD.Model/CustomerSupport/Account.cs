using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TMCWD.Model.CustomerSupport.Interfaces;

namespace TMCWD.Model.CustomerSupport
{
    public class Account : IAccount
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Customer Id")]
        public int CustomerId { get; set; }
        [DisplayName("Customer Number")]
        public string AccountNumber { get; set; } = string.Empty;
        [DisplayName("Meter Number")]
        public string MeterNumber { get; set; } = string.Empty;
        [DisplayName("Address")]
        public string Address { get; set; } = string.Empty;
        [DisplayName("Current")]
        public bool IsCurrentAddress { get; set; }
        [DisplayName("Date Enrolled")]
        public DateTime DateCreated { get; set; }
        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
        [DisplayName("Created By")]
        public int CreatedBy { get; set; }
        [DisplayName("Updated By")]
        public int UpdatedBy { get; set; }
    }
}
