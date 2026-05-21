using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.CustomerSupport.Interfaces;

namespace TMCWD.Model.CustomerSupport
{
    public class Material : IMaterial
    {

        public Material() { }

        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int RequestId { get; set; }
        public int RequestedQuantity { get; set; }
        public float UnitCost { get; set; }
        public float NewUnitCost { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public float Amount => (NewUnitCost > 0 ? NewUnitCost : UnitCost) * RequestedQuantity;
    }
}
