using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.Engineering.Interfaces;

namespace TMCWD.Model.Engineering
{
    public class Inventory : IInventory
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Division { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Amount { 
            get
            {
                return CalculateAmount();
            }
        }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public decimal CalculateAmount()
        {
            return this.Quantity * this.UnitCost;
        }
    }
}
