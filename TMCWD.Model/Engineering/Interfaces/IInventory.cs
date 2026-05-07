using System;
using System.Collections.Generic;
using System.Text;

namespace TMCWD.Model.Engineering.Interfaces
{
    internal interface IInventory
    {
        #region properties

        public int Id { get; set; }

        public string Name { get; set; }

        public int Division { get; set; }

        public string Unit { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitCost { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }


        #endregion

        #region methods

        protected decimal CalculateAmount();

        #endregion
    }
}
