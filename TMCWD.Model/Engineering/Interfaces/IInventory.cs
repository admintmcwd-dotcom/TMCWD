using System;
using System.Collections.Generic;
using System.Text;

namespace TMCWD.Model.Engineering.Interfaces
{
    internal interface IInventory
    {
        #region properties

        public decimal Id { get; set; }

        public string Name { get; set; }

        public string ControlNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }


        #endregion
    }
}
