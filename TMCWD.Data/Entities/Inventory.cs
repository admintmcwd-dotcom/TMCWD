using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{
    [Table("inventory")]
    public class Inventory
    {
        [Key, Column("Id")]
        public System.Int64 Id { get; set; }

        [Required, MaxLength(255), Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Required, Column("Division")]
        public int Division { get; set; }

        [Required, MaxLength(25), Column("Unit")]
        public string Unit { get; set; } = string.Empty;

        [Required, Column("Quantity")]
        public decimal Quantity { get; set; }

        [Required, Column("UnitCost")]
        public decimal UnitCost { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("CreatedBy")]
        public System.Int64 CreatedBy { get; set; }

        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }

        [Column("UpdatedBy")]
        public System.Int64 UpdatedBy { get; set; }
    }
}
