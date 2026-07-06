using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{

    [Table("penalty_type")]
    public class PenaltyType
    {

        [Key, Column("Id")]
        public System.Int64 Id { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("CreatedBy")]
        public System.Int64 CreatedBy { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("UpdatedBy")]
        public System.Int64 UpdatedBy { get; set; }

        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }

    }
}
