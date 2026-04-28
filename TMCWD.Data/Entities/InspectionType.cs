using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{
    [Table("inspection_types")]
    public class InspectionType
    {

        #region constructors

        public InspectionType() { }

        #endregion

        #region properties

        [Key]
        [Column("Id")]
        public decimal Id { get; set; }

        [Required, MaxLength(150)]
        [Column("Name")]
        public string Name { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }

        #endregion
    }
}
