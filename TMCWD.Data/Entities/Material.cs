using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TMCWD.Data.Entities
{

    [Table("materials")]
    public class Material
    {

        #region constructors

        public Material() { }

        #endregion

        #region members

        [Key, Column("Id")]
        public System.Int64 Id { get; set; }

        [Required, Column("InventoryId")]
        public System.Int64 InventoryId { get; set; }

        [Required, Column("RequestedQuantity")]
        public int RequestedQuantity { get; set; } = 0;

        [Column("NewUnitCost")]
        public float NewUnitCost { get; set; }

        [Required, Column("CreatedBy")]
        public System.Int64 CreatedBy { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("UpdatedBy")]
        public System.Int64 UpdatedBy { get; set; }

        [Column("UpdatedBy")]
        public DateTime DateUpdated { get; set; }


        #endregion

    }
}
