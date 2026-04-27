using System;
using System.Collections.Generic;
using System.Text;

namespace TMCWD.Model.CustomerSupport.Interfaces
{
    public interface ICustomer
    {

        #region properties

        public System.Int64 Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Middlename { get; set; }

        public string Fullname
        {
            get
            {
                return $"{Firstname} {Middlename} {Lastname}";
            }
        }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsActive { get; set; }

        #endregion

    }
}
